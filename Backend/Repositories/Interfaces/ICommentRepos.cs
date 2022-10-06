using Models;
using DataTransferObject;

namespace Repositories{
    public interface ICommentRepos : IBaseRepos{

        // GET
        // public IEnumerable<Comment> GetCommentsByUser(Guid _guid);
        // public IEnumerable<Comment> GetCommentsByPost(Guid _guid);
        public Comment GetCommentById(Guid _guid);

        // POST
        public Comment CreateComment(CommentEntryDto _input);


        // PUT
        public Comment UpdateComment(Guid _guid, CommentEntryDto _input);
    }
}