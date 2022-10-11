using Models;
using DataTransferObject;

namespace Repositories{
    public interface IBaseRepos{
        public Task<bool> CommentExists(Guid _Id);
        public Task<bool> UserExists(Guid _Id);
        public Task<bool> PostExists(Guid _Id);


        public Task<bool> HardDeletePostById(Guid _Id);
        public Task<bool> SoftDeleteCommentById(Guid _Id);
        public Task<bool> HardDeleteCommentById(Guid _Id);
        public Task<bool> SoftDeleteUserById(Guid _Id);
        public Task<bool> HardDeleteUserById(Guid _Id);
    }
}