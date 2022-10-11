using Models;
using DataTransferObject;

namespace Repositories{
    public interface IPostRepos : IBaseRepos{
        // GET
        public IEnumerable<Post> GetPosts();
        public IEnumerable<Post> GetPostsByUser(Guid _Id);
        public IEnumerable<Comment> GetPostsComments(Guid _Id);
        public Post GetPostById(Guid _Id);

        // POST
        public Post CreatePost(PostEntryDto _input);


        // PUT
        public Post UpdatePost(Guid _Id, PostEntryDto _input);
    }
}