using System;
using Models;
using DataTransferObject;
using DataTransferObject.Base;
using AutoMapper;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers{
    [Route("api/[Controller]")]
    [ApiController]
    public class PostController : Controller{
        
        private readonly IPostRepos _repos;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly LinkGenerator _linkGenerator;

        public PostController(IPostRepos reposContext, IMapper mapperContext, ILogger<Post> loggerContext, LinkGenerator linkContext){
            _repos = reposContext ?? throw new ArgumentNullException(nameof(reposContext));
            _mapper = mapperContext ?? throw new ArgumentNullException(nameof(mapperContext));
            _logger = loggerContext ?? throw new ArgumentNullException(nameof(loggerContext));
            _linkGenerator = linkContext ?? throw new ArgumentNullException(nameof(linkContext));
        }

        [HttpGet("")]
        public IActionResult GetPosts(){
            var _result = _repos.GetPosts();
            var _mapped = _mapper.Map<IEnumerable<PostSimpleDto>>(_result);
            _logger.LogInformation("Returned all posts",DateTime.UtcNow.ToLongTimeString());

            return _result != null ? Ok(_mapped) : NotFound();
        }

        [HttpGet("{_guid}")]
        public async Task<IActionResult> GetPostById(Guid _guid){
            if(await _repos.PostExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetPostById(_guid);
            var _mapped = _mapper.Map<PostDto>(_result);
            _mapped.Links = CreateHateosLinks(_guid,"");
            _logger.LogInformation("Returned posts withGuid:" + _guid,DateTime.UtcNow.ToLongTimeString());

            return _result != null ? Ok(_mapped) : NotFound();
        }

        [HttpGet("{_guid}/Comments")]
        public IActionResult GetPostComments(Guid _guid){
            // missing validatioon ERROR

            var _result = _repos.GetPostsComments(_guid);
            var _mapped = _mapper.Map<IEnumerable<CommentSimpleUserDto>>(_result);
            _logger.LogInformation("Returned posts comments withGuid:" + _guid,DateTime.UtcNow.ToLongTimeString());

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpDelete("{_guid}")]
        public async Task<IActionResult> DeletePost(Guid _guid){
            if(await _repos.PostExists(_guid) == false)
                return NotFound();

            var _result = _repos.HardDeletePostById(_guid).Result;

            if(_result)
                _logger.LogInformation("Deleted post : " + _guid,DateTime.UtcNow.ToLongTimeString());

            return _result ? Ok() : BadRequest();
        }
        
        [HttpPost("")]
        public ActionResult CreatePost([FromBody] PostEntryDto _input){
            var _result = _repos.CreatePost(_input);

            if(_result == null)
                return BadRequest("No");

            _logger.LogInformation("Created post : " + _result.guId,DateTime.UtcNow.ToLongTimeString());

            var _mapped = _mapper.Map<PostDto>(_result);
            _mapped.Links = CreateHateosLinks(_result.guId,"");

            return Ok(_mapped);
        }

        [HttpPut("{_guid}")]
        public async Task<IActionResult> UpdatePost(Guid _guid,[FromBody] PostEntryDto _input){
            if(await _repos.PostExists(_guid) == false)
                return NotFound();

            var _result = _repos.UpdatePost(_guid,_input);

            if(_result == null)
                return BadRequest("No");

            _logger.LogInformation("Updated post : " + _guid,DateTime.UtcNow.ToLongTimeString());

            var _mapped = _mapper.Map<PostDto>(_result);
            _mapped.Links = CreateHateosLinks(_guid,"");
            
            return Ok(_mapped);
        }

        private List<Link> CreateHateosLinks(Guid _guid, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetPostById), values: new {_guid}),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetPostComments), values: new {_guid}),
                "self_comments",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeletePost), values: new {_guid}),
                "self_delete",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdatePost), values: new {_guid}),
                "update_post",
                "PUT")
            };
            return links;
        }
    }
}