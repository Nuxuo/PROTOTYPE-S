using Models;
using DataTransferObject;

namespace Repositories{
    public interface IUserRepos : IBaseRepos{

        // GET
        public IEnumerable<User> GetUsers(); 
        public User GetUserById(Guid _Id);
        public IEnumerable<Comment> GetUsersComments(Guid _Id);
        public IEnumerable<Post> GetUsersPosts(Guid _Id);
        public IEnumerable<UserTag> GetUsersTags(Guid _Id);
        public IEnumerable<PostRatingDto> GetUsersTargetedPosts(Guid _Id, int ammount);
        public IEnumerable<UserPostRelation> GetUsersUserPostRelation(Guid _Id);
        public IEnumerable<UserCommentRelation> GetUsersUserCommentRelation(Guid _Id);

        // public IEnumerable<User> GetUsersByLastnameSearch(string lastname); 
        // public User GetUserByLastnameSearch(string lastname);

        // POST
        public User CreateUser(UserEntryDto _entry);


        // PUT
        public User UpdateUser(Guid _Id, UserEntryDto _entry);
        public string ToggleUserPostRelation(Guid _UserId, Guid _PostId, bool _status);
        public string ToggleUserCommentRelation(Guid _UserId, Guid _CommentId, bool _status);

        public Task<bool> SoftDeleteUserById(Guid _Id);
    }
}