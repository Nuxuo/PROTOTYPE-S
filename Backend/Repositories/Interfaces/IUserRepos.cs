using Models;
using DataTransferObject;

namespace Repositories{
    public interface IUserRepos : IBaseRepos{

        // GET
        public IEnumerable<User> GetUsers(); 
        public User GetUserById(Guid _guid);
        public IEnumerable<Comment> GetUsersComments(Guid _guid);
        public IEnumerable<Post> GetUsersPosts(Guid _guid);
        public IEnumerable<UserTag> GetUsersTags(Guid _guid);
        public IEnumerable<PostRatingDto> GetUsersTargetedPosts(Guid _guid, int ammount);
        public IEnumerable<UserPostRelation> GetUsersUserPostRelation(Guid _guid);
        public IEnumerable<UserCommentRelation> GetUsersUserCommentRelation(Guid _guid);

        // public IEnumerable<User> GetUsersByLastnameSearch(string lastname); 
        // public User GetUserByLastnameSearch(string lastname);

        // POST
        public User CreateUser(UserEntryDto _input);


        // PUT
        public User UpdateUser(Guid _guid, UserEntryDto _input);
        public string ToggleUserPostRelation(Guid _UserGuid, Guid _PostGuid, bool _status);
        public string ToggleUserCommentRelation(Guid _UserGuid, Guid _CommentGuid, bool _status);
    }
}