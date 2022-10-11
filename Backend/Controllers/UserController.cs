using System;
using Models;
using AutoMapper;
using Repositories;
using DataTransferObject;
using DataTransferObject.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : Controller{
        private readonly IUserRepos _repos;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly LinkGenerator _linkGenerator;

        public UserController(IUserRepos reposContext, IMapper mapperContext, ILogger<User> loggerContext, LinkGenerator linkContext){
            _repos = reposContext ?? throw new ArgumentNullException(nameof(reposContext));
            _mapper = mapperContext ?? throw new ArgumentNullException(nameof(mapperContext));
            _logger = loggerContext ?? throw new ArgumentNullException(nameof(loggerContext));
            _linkGenerator = linkContext ?? throw new ArgumentNullException(nameof(linkContext));
        }

        [HttpGet]
        public IActionResult GetUsers(){
            var _result = _repos.GetUsers();
            var _mapped = _mapper.Map<IEnumerable<UserSimpleDto>>(_result);

            LogInformation("Returned Users");

            return _result != null ? Ok(_mapped) : NotFound();
        }

        [HttpGet("{_Id}")]
        public async Task<IActionResult> GetUserById(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetUserById(_Id);
            var _mapped = _mapper.Map<UserDto>(_result);
            _mapped.Links = CreateHateosLinks(_Id);

            LogInformation("Returned User ", _Id);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_Id}/Posts")]
        public async Task<IActionResult> GetUserPosts(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetUsersPosts(_Id);
            var _mapped = _mapper.Map<IEnumerable<PostSimpleDto>>(_result);

            LogInformation("Returned User posts ", _Id);

            return _result != null ? Ok(_mapped) : BadRequest();
        }
   
        [HttpGet("{_Id}/Comments")]
        public async Task<IActionResult> GetUserComments(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetUsersComments(_Id);
            var _mapped = _mapper.Map<IEnumerable<CommentSimplePostDto>>(_result);

            LogInformation("Returned User comments ", _Id);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_Id}/Tags")]
        public async Task<IActionResult> GetUserTags(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetUsersTags(_Id);
            var _mapped = _mapper.Map<IEnumerable<TagLikesDto>>(_result);

            LogInformation("Returned User tags ", _Id);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_Id}/Relation/Posts")]
        public async Task<IActionResult> GetUserPostRelations(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetUsersUserPostRelation(_Id);
            var _mapped = _mapper.Map<IEnumerable<UserPostRelationDto>>(_result);

            LogInformation("Returned User post relation ", _Id);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_Id}/Relation/Comments")]
        public async Task<IActionResult> GetUserCommentRelations(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetUsersUserCommentRelation(_Id);
            var _mapped = _mapper.Map<IEnumerable<UserCommentRelationDto>>(_result);

            LogInformation("Returned User comment relation ", _Id);

            return _result != null ? Ok(_mapped) : BadRequest();
        }


        [HttpGet("{_Id}/TargetedPosts/{ammount}")]
        public async Task<IActionResult> GetUserTargetedPosts(Guid _Id, int ammount){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetUsersTargetedPosts(_Id, ammount);

            if(_result == null ||  _result.Count() <= 0)
                return BadRequest("ERROR 1002");

            // var _mapped = _mapper.Map<IEnumerable<PostDto>>(_result);
            
            LogInformation("Returned "+ammount+" targeted posts for user ", _Id);

            return _result != null ? Ok(_result) : BadRequest();
        }

        [HttpDelete("Soft/{_Id}")]
        public async Task<IActionResult> SoftDeleteUser(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            if(!_repos.SoftDeleteUserById(_Id).Result)
                return BadRequest();

            LogInformation("Soft deleted user ", _Id);

            return Ok();
        }

        [HttpDelete("Hard/{_Id}")]
        public async Task<IActionResult> HardDeleteUser(Guid _Id){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            if(!_repos.HardDeleteUserById(_Id).Result)
                return BadRequest();

            LogInformation("Hard deleted user ", _Id);

            return Ok();
        }
        
        [HttpPost("")]
        public ActionResult CreateUser([FromBody] UserEntryDto _input){
            var _result = _repos.CreateUser(_input);

            if(_result == null)
                return BadRequest("No");

            var _mapped = _mapper.Map<UserDto>(_result);
            _mapped.Links = CreateHateosLinks(_result.Id,"");

            LogInformation("Created user ", _result.Id);

            return Ok(_mapped);
        }

        [HttpPut("{_Id}")]
        public async Task<IActionResult> UpdateUser(Guid _Id,[FromBody] UserEntryDto _input){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            var _result = _repos.UpdateUser(_Id,_input);

            if(_result == null)
                return BadRequest("No");

            LogInformation("Updated post ", _Id);

            var _mapped = _mapper.Map<UserDto>(_result);
            _mapped.Links = CreateHateosLinks(_Id,"");
            
            return Ok(_mapped);
        }

        [HttpPut("{_Id}/togglePostRelation/{PostId}/{status}")]
        public async Task<IActionResult> togglePostRelation(Guid _Id,Guid PostId,bool status){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            if(await _repos.PostExists(PostId) == false)
                return NotFound();

            var _result = _repos.ToggleUserPostRelation(_Id,PostId,status);

            if(_result == null)
                return BadRequest("ERROR #1005");

            LogInformation("Toggled relation with user and post", _Id);
            
            return Ok(_result);
        }

        [HttpPut("{_Id}/toggleCommentRelation/{CommentId}/{status}")]
        public async Task<IActionResult> toggleCommentRelation(Guid _Id, Guid CommentId, bool status){
            if(await _repos.UserExists(_Id) == false)
                return NotFound();

            if(await _repos.CommentExists(CommentId) == false)
                return NotFound();

            var _result = _repos.ToggleUserCommentRelation(_Id,CommentId,status);

            if(_result == null)
                return BadRequest("ERROR #1006");

            LogInformation("Toggled relation with user and Comment", _Id);
            
            return Ok(_result);
        }
        
        
        private void LogInformation(string message, Guid _Id= new Guid()){
            // if(id == 0)
            //     _logger.LogInformation(message,DateTime.UtcNow.ToLongTimeString());

            _logger.LogInformation(message , _Id, DateTime.UtcNow.ToLongTimeString());
        }

        private List<Link> CreateHateosLinks(Guid _Id, string fields = "")
        {
            string ammount = "&ammount";

            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserById), values: new {_Id}),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserComments), values: new {_Id}),
                "self_comments",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserPosts), values: new {_Id}),
                "self_posts",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserTags), values: new {_Id}),
                "self_tags",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserPostRelations), values: new {_Id}),
                "self_likes",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserTargetedPosts), values: new {_Id, ammount }),
                "targeted_posts",
                "GET")
            };
            return links;
        }
    }
}