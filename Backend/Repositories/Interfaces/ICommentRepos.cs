using Models;
using DataTransferObject;

namespace Repositories{
    public interface ICommentRepos : IBaseRepos{

        // GET
        // public IEnumerable<Comment> GetCommentsByUser(Guid _Id);
        // public IEnumerable<Comment> GetCommentsByPost(Guid _Id);
        public Comment GetCommentById(Guid _Id);

        // POST
        public Comment CreateComment(CommentEntryDto _input);


        // PUT
        public Comment UpdateComment(Guid _Id, CommentEntryDto _input);
    }
}