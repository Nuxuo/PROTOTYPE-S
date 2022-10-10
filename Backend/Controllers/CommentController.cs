using Models;
using DataTransferObject;
using DataTransferObject.Base;
using Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Controllers{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommentController : Controller{
        
        private readonly ICommentRepos _repos;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly LinkGenerator _linkGenerator;

        public CommentController(ICommentRepos reposContext, IMapper mapperContext, ILogger<Comment> loggerContext, LinkGenerator linkContext){
            _repos = reposContext ?? throw new ArgumentNullException(nameof(reposContext));
            _mapper = mapperContext ?? throw new ArgumentNullException(nameof(mapperContext));
            _logger = loggerContext ?? throw new ArgumentNullException(nameof(loggerContext));
            _linkGenerator = linkContext ?? throw new ArgumentNullException(nameof(linkContext));
        }

        [HttpGet("{_guid}")]
        public async Task<ActionResult<Comment>> GetComment(Guid _guid){
            if(await _repos.CommentExists(_guid) == false)
                return NotFound();

            var _result = _repos.GetCommentById(_guid);
            var _mapped = _mapper.Map<CommentDto>(_result);
            _mapped.Links = CreateLinksForComment(_guid,"");
            
            _logger.LogInformation("Returned comment : " + _guid,DateTime.UtcNow.ToLongTimeString());

            return _result != null ? Ok(_mapped) : BadRequest();
        }

        [HttpDelete("Soft/{_guid}")]
        public async Task<ActionResult> SoftDeleteComment(Guid _guid){
            if(await _repos.CommentExists(_guid) == false)
                return NotFound();

            var _result = _repos.SoftDeleteCommentById(_guid).Result;

            if(_result)
                _logger.LogInformation("Soft Deleted comment : " + _guid,DateTime.UtcNow.ToLongTimeString());

            return _result ? Ok() : BadRequest();
        }

        [HttpDelete("Hard/{_guid}")]
        public async Task<ActionResult> HardDeleteComment(Guid _guid){
            if(await _repos.CommentExists(_guid) == false)
                return NotFound();

            var _result = _repos.HardDeleteCommentById(_guid).Result;

            if(_result)
                _logger.LogInformation("Hard Deleted comment : " + _guid,DateTime.UtcNow.ToLongTimeString());

            return _result ? Ok() : BadRequest();
        }

        [HttpPost("")]
        public ActionResult CreateComment([FromBody] CommentEntryDto _input){
            var _result = _repos.CreateComment(_input);

            if(_result == null)
                return BadRequest();

            _logger.LogInformation("Created comment : " + _result.guId,DateTime.UtcNow.ToLongTimeString());

            var _mapped = _mapper.Map<CommentDto>(_result);
            _mapped.Links = CreateLinksForComment(_result.guId,"");
            return Ok(_mapped);
        }

        [HttpPut("{_guid}")]
        public async Task<ActionResult> UpdateComment(Guid _guid,[FromBody] CommentEntryDto _input){
            if(await _repos.CommentExists(_guid) == false)
                return NotFound();

            var _result = _repos.UpdateComment(_guid,_input);

            if(_result == null)
                return BadRequest();

            _logger.LogInformation("Updated comment : " + _guid,DateTime.UtcNow.ToLongTimeString());

            var _mapped = _mapper.Map<CommentDto>(_result);
            _mapped.Links = CreateLinksForComment(_guid,"");
            
            return Ok(_mapped);
        }


        private List<Link> CreateLinksForComment(Guid _guid, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetComment), values: new { _guid, fields }),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(SoftDeleteComment), values: new {_guid}),
                "soft_delete_comment",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(HardDeleteComment), values: new {_guid}),
                "hard_delete_comment",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateComment), values: new {_guid}),
                "update_comment",
                "PUT")
            };
            return links;
        }
    }
}