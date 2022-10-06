using Models;
using DataTransferObject;

namespace Repositories{
    public interface IBaseRepos{
        public bool CommentExists(Guid _guid);
        public bool UserExists(Guid _guid);
        public bool PostExists(Guid _guid);


        public bool HardDeletePostById(Guid _guid);
        public bool SoftDeleteCommentById(Guid _guid);
        public bool HardDeleteCommentById(Guid _guid);
        public bool SoftDeleteUserById(Guid _guid);
        public bool HardDeleteUserById(Guid _guid);
    }
}