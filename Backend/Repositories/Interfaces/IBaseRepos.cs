using Models;
using DataTransferObject;

namespace Repositories{
    public interface IBaseRepos{
        public Task<bool> CommentExists(Guid _guid);
        public Task<bool> UserExists(Guid _guid);
        public Task<bool> PostExists(Guid _guid);


        public Task<bool> HardDeletePostById(Guid _guid);
        public Task<bool> SoftDeleteCommentById(Guid _guid);
        public Task<bool> HardDeleteCommentById(Guid _guid);
        public Task<bool> SoftDeleteUserById(Guid _guid);
        public Task<bool> HardDeleteUserById(Guid _guid);
    }
}