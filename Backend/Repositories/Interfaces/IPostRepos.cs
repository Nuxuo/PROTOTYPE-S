using Models;
using DataTransferObject;

namespace Repositories{
    public interface IPostRepos : IBaseRepos{
        // GET
        public IEnumerable<Post> GetPosts();
        public IEnumerable<Post> GetPostsByUser(Guid _guid);
        public IEnumerable<Comment> GetPostsComments(Guid _guid);
        public Post GetPostById(Guid _guid);

        // POST
        public Post CreatePost(PostEntryDto _input);


        // PUT
        public Post UpdatePost(Guid _guid, PostEntryDto _input);
    }
}