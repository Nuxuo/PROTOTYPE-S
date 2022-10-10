using Models;
using DataTransferObject;
using DataTransferObject.Base;
using Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Controllers{
    [Route("api/[Controller]")]
    [ApiController]
    public class TestingController : Controller{
        
        private readonly ITestingRepos _repos;
        private readonly LinkGenerator _linkGenerator;

        public TestingController(ITestingRepos reposContext, LinkGenerator linkContext){
            _repos = reposContext ?? throw new ArgumentNullException(nameof(reposContext));
            _linkGenerator = linkContext ?? throw new ArgumentNullException(nameof(linkContext));
        }

        [HttpPost("Posts/{ammount}")]
        public async Task<ActionResult> CreatePosts(int ammount){
            if(await _repos.CreatePosts(ammount) == false)
                return BadRequest();

            return Ok("Created " + ammount + " posts.");
        }

        [HttpPost("Comments/{ammount}")]
        public async Task<ActionResult> CreateComments(int ammount){
            if(await _repos.CreateComments(ammount) == false)
                return BadRequest();

            return Ok("Created " + ammount + " comments");
        }

        [HttpPost("User/Comments/{ammount}")]
        public async Task<ActionResult> CreateUserCommentRelation(int ammount){
            if(await _repos.UserRelationComments(ammount) == false)
                return BadRequest();

            return Ok("Created " + ammount + " comments relations");
        }

        [HttpPost("User/Posts/{ammount}")]
        public async Task<ActionResult> CreateUserPostsRelation(int ammount){
            if(await _repos.UserRelationPosts(ammount) == false)
                return BadRequest();

            return Ok("Created " + ammount + " posts relations");
        }

        [HttpPost("Simulate/{ammount}")]
        public async Task<ActionResult> SimulateData(int ammount){
            if(await _repos.CreatePosts(ammount)==false)
                return BadRequest();
            if(await _repos.CreateComments(ammount * 3)==false)
                return BadRequest();
            if(await _repos.UserRelationPosts(ammount * 10)==false)
                return BadRequest();
            if(await _repos.UserRelationComments(ammount * 10)==false)
                return BadRequest();
            
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePosts(){
            return Ok(await _repos.TruncateData());
        }
        
    }
}