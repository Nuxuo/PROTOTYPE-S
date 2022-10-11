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

        [HttpGet("{_Id}")]
        public async Task<IActionResult> GetPostById(Guid _Id){
            if(await _repos.PostExists(_Id) == false)
                return NotFound();

            var _result = _repos.GetPostById(_Id);
            var _mapped = _mapper.Map<PostDto>(_result);
            _mapped.Links = CreateHateosLinks(_Id,"");
            _logger.LogInformation("Returned posts withGuid:" + _Id,DateTime.UtcNow.ToLongTimeString());

            return _result != null ? Ok(_mapped) : NotFound();
        }

        [HttpGet("{_Id}/Comments")]
        public IActionResult GetPostComments(Guid _Id){
            // missing validatioon ERROR

            var _result = _repos.GetPostsComments(_Id);
            var _mapped = _mapper.Map<IEnumerable<CommentSimpleUserDto>>(_result);
            _logger.LogInformation("Returned posts comments withGuid:" + _Id,DateTime.UtcNow.ToLongTimeString());

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpDelete("{_Id}")]
        public async Task<IActionResult> DeletePost(Guid _Id){
            if(await _repos.PostExists(_Id) == false)
                return NotFound();

            var _result = _repos.HardDeletePostById(_Id).Result;

            if(_result)
                _logger.LogInformation("Deleted post : " + _Id,DateTime.UtcNow.ToLongTimeString());

            return _result ? Ok() : BadRequest();
        }
        
        [HttpPost("")]
        public ActionResult CreatePost([FromBody] PostEntryDto _entry){
            var _result = _repos.CreatePost(_entry);

            if(_result == null)
                return BadRequest("No");

            _logger.LogInformation("Created post : " + _result.Id,DateTime.UtcNow.ToLongTimeString());

            var _mapped = _mapper.Map<PostDto>(_result);
            _mapped.Links = CreateHateosLinks(_result.Id,"");

            return Ok(_mapped);
        }

        [HttpPut("{_Id}")]
        public async Task<IActionResult> UpdatePost(Guid _Id,[FromBody] PostEntryDto _entry){
            if(await _repos.PostExists(_Id) == false)
                return NotFound();

            var _result = _repos.UpdatePost(_Id,_entry);

            if(_result == null)
                return BadRequest("No");

            _logger.LogInformation("Updated post : " + _Id,DateTime.UtcNow.ToLongTimeString());

            var _mapped = _mapper.Map<PostDto>(_result);
            _mapped.Links = CreateHateosLinks(_Id,"");
            
            return Ok(_mapped);
        }

        private List<Link> CreateHateosLinks(Guid _Id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetPostById), values: new {_Id}),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetPostComments), values: new {_Id}),
                "self_comments",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeletePost), values: new {_Id}),
                "self_delete",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdatePost), values: new {_Id}),
                "update_post",
                "PUT")
            };
            return links;
        }
    }
}