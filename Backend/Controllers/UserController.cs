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

        [HttpGet("{_guid}")]
        public async Task<IActionResult> GetUserById(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetUserById(_guid);
            var _mapped = _mapper.Map<UserDto>(_result);
            _mapped.Links = CreateHateosLinks(_guid);

            LogInformation("Returned User ", _guid);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_guid}/Posts")]
        public async Task<IActionResult> GetUserPosts(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetUsersPosts(_guid);
            var _mapped = _mapper.Map<IEnumerable<PostSimpleDto>>(_result);

            LogInformation("Returned User posts ", _guid);

            return _result != null ? Ok(_mapped) : BadRequest();
        }
   
        [HttpGet("{_guid}/Comments")]
        public async Task<IActionResult> GetUserComments(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetUsersComments(_guid);
            var _mapped = _mapper.Map<IEnumerable<CommentSimplePostDto>>(_result);

            LogInformation("Returned User comments ", _guid);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_guid}/Tags")]
        public async Task<IActionResult> GetUserTags(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetUsersTags(_guid);
            var _mapped = _mapper.Map<IEnumerable<TagLikesDto>>(_result);

            LogInformation("Returned User tags ", _guid);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_guid}/Relation/Posts")]
        public async Task<IActionResult> GetUserPostRelations(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetUsersUserPostRelation(_guid);
            var _mapped = _mapper.Map<IEnumerable<UserPostRelationDto>>(_result);

            LogInformation("Returned User post relation ", _guid);

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpGet("{_guid}/Relation/Comments")]
        public async Task<IActionResult> GetUserCommentRelations(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetUsersUserCommentRelation(_guid);
            var _mapped = _mapper.Map<IEnumerable<UserCommentRelationDto>>(_result);

            LogInformation("Returned User comment relation ", _guid);

            return _result != null ? Ok(_mapped) : BadRequest();
        }


        [HttpGet("{_guid}/TargetedPosts/{ammount}")]
        public async Task<IActionResult> GetUserTargetedPosts(Guid _guid, int ammount){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetUsersTargetedPosts(_guid, ammount);

            if(_result == null ||  _result.Count() <= 0)
                return BadRequest("ERROR 1002");

            // var _mapped = _mapper.Map<IEnumerable<PostDto>>(_result);
            
            LogInformation("Returned "+ammount+" targeted posts for user ", _guid);

            return _result != null ? Ok(_result) : BadRequest();
        }

        [HttpDelete("Soft/{_guid}")]
        public async Task<IActionResult> SoftDeleteUser(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            if(!_repos.SoftDeleteUserById(_guid).Result)
                return BadRequest();

            LogInformation("Soft deleted user ", _guid);

            return Ok();
        }

        [HttpDelete("Hard/{_guid}")]
        public async Task<IActionResult> HardDeleteUser(Guid _guid){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            if(!_repos.HardDeleteUserById(_guid).Result)
                return BadRequest();

            LogInformation("Hard deleted user ", _guid);

            return Ok();
        }
        
        [HttpPost("")]
        public ActionResult CreateUser([FromBody] UserEntryDto _input){
            var _result = _repos.CreateUser(_input);

            if(_result == null)
                return BadRequest("No");

            var _mapped = _mapper.Map<UserDto>(_result);
            _mapped.Links = CreateHateosLinks(_result.guId,"");

            LogInformation("Created user ", _result.guId);

            return Ok(_mapped);
        }

        [HttpPut("{_guid}")]
        public async Task<IActionResult> UpdateUser(Guid _guid,[FromBody] UserEntryDto _input){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            var _result = _repos.UpdateUser(_guid,_input);

            if(_result == null)
                return BadRequest("No");

            LogInformation("Updated post ", _guid);

            var _mapped = _mapper.Map<UserDto>(_result);
            _mapped.Links = CreateHateosLinks(_guid,"");
            
            return Ok(_mapped);
        }

        [HttpPut("{_guid}/togglePostRelation/{PostGuid}/{status}")]
        public async Task<IActionResult> togglePostRelation(Guid _guid,Guid PostGuid,bool status){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            if(await _repos.PostExists(PostGuid) == false)
                return NotFound();

            var _result = _repos.ToggleUserPostRelation(_guid,PostGuid,status);

            if(_result == null)
                return BadRequest("ERROR #1005");

            LogInformation("Toggled relation with user and post", _guid);
            
            return Ok(_result);
        }

        [HttpPut("{_guid}/toggleCommentRelation/{CommentGuid}/{status}")]
        public async Task<IActionResult> toggleCommentRelation(Guid _guid, Guid CommentGuid, bool status){
            if(await _repos.UserExists(_guid) == false)
                return NotFound();

            if(await _repos.CommentExists(CommentGuid) == false)
                return NotFound();

            var _result = _repos.ToggleUserCommentRelation(_guid,CommentGuid,status);

            if(_result == null)
                return BadRequest("ERROR #1006");

            LogInformation("Toggled relation with user and Comment", _guid);
            
            return Ok(_result);
        }
        
        
        private void LogInformation(string message, Guid _guid= new Guid()){
            // if(id == 0)
            //     _logger.LogInformation(message,DateTime.UtcNow.ToLongTimeString());

            _logger.LogInformation(message , _guid, DateTime.UtcNow.ToLongTimeString());
        }

        private List<Link> CreateHateosLinks(Guid _guid, string fields = "")
        {
            string ammount = "&ammount";

            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserById), values: new {_guid}),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserComments), values: new {_guid}),
                "self_comments",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserPosts), values: new {_guid}),
                "self_posts",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserTags), values: new {_guid}),
                "self_tags",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserPostRelations), values: new {_guid}),
                "self_likes",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetUserTargetedPosts), values: new {_guid, ammount }),
                "targeted_posts",
                "GET")
            };
            return links;
        }
    }
}