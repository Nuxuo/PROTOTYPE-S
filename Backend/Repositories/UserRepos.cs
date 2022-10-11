using Microsoft.EntityFrameworkCore;
using Context;
using Models;
using AutoMapper;
using DataTransferObject;

namespace Repositories{
    public class UserRepos : BaseRepos, IUserRepos{
        public UserRepos(DatabaseContext context, IMapper mapperContext){
            _mapper = mapperContext ?? throw new ArgumentNullException(nameof(mapperContext));
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public IEnumerable<User> GetUsers(){
            return _context.Users.Where(x=>!x.SoftDeleted).ToList();
        }
        public User GetUserById(Guid id){
            return _context.Users
            .FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<UserPostRelation> GetUsersUserPostRelation(Guid _Id){
            return _context.UserPostRelations.Where(x=>x.UserId == _Id).ToList();
        }
        public IEnumerable<UserCommentRelation> GetUsersUserCommentRelation(Guid _Id){
            return _context.UserCommentRelations.Where(x=>x.UserId == _Id).ToList();
        }
        public IEnumerable<Comment> GetUsersComments(Guid _Id){
            return _context.Comments.Where(x => x.UserId == _Id).ToList();
        }
        public IEnumerable<Post> GetUsersPosts(Guid _Id){
            return _context.Posts
                .Include(x=>x.Tags)
                    .ThenInclude(x=>x.Tag)
                .Where(x => x.UserId == _Id)
            .ToList();
        }
        public IEnumerable<UserTag> GetUsersTags(Guid _Id){
            return _context.UserTags
                .Include(x=>x.Tag)
                    .Where(x => x.UserId == _Id).ToList();
        }
        public IEnumerable<PostRatingDto> GetUsersTargetedPosts(Guid _Id, int ammount){
            List<PostRatingDto> _returnList = new List<PostRatingDto>();

            var _Posts = _context.Posts
                .Include(x=>x.Tags)
                    .ThenInclude(x=>x.Tag)
                .OrderBy(x=>x.CreatedDate)
                .Take(100);

            var _UserRelation = GetUsersUserPostRelation(_Id);
            var _UserTags = _context.UserTags.Where(x=>x.UserId == _Id).ToList();

            foreach (Post _p in _Posts){
                if(_UserRelation.FirstOrDefault(x=>x.PostId ==_p.Id) == null){
                    int _rating = 0;
                    foreach (PostTag _pt in _p.Tags){
                        UserTag _userTag = _UserTags.FirstOrDefault(x=>x.TagId == _pt.TagId);
                        if(_userTag == null)
                            break;
                        _rating = _rating + _userTag.Likes; 
                    }

                    var _mapped = _mapper.Map<PostRatingDto>(_p);
                    _mapped.Rating = _rating;
                    _returnList.Add(_mapped);
                }
            }

            IEnumerable<PostRatingDto> _IreturnList = _returnList.OrderByDescending(x=>x.Rating);
            return _IreturnList.Take(ammount);
        }
        public async Task<bool> SoftDeleteUserById(Guid _Id){
            if(await UserExists(_Id)){
                var _User = await _context.Users.FirstOrDefaultAsync(x=>x.Id == _Id);
                _User.SoftDeleted = true;
                _context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }
        public User CreateUser(UserEntryDto _entry){
            User _User = new User(_entry);

            _context.Users.Add(_User);
            _context.SaveChanges();
            
            return _User;
        }
        public User UpdateUser(Guid _Id, UserEntryDto _entry){
            var _User = _context.Users.Find(_Id);

            _User.Firstname = _entry.Firstname;
            _User.Lastname = _entry.Lastname;
            _User.Email = _entry.Email;           
            _User.Password = _entry.Password; 

            _context.SaveChanges();
            return _User;
        }
        public string ToggleUserPostRelation(Guid _UserId, Guid _PostId, bool _status){
            UserPostRelation _userpostRelation = _context.UserPostRelations.FirstOrDefault(x=>x.PostId == _PostId && x.UserId == _UserId);
            Post _post = _context.Posts.Include(x=>x.Tags).FirstOrDefault(x => x.Id == _PostId);
            User _user = _context.Users.Include(x=>x.Tags).Include(x=>x.UserPostRelation).FirstOrDefault(x => x.Id == _UserId);
            string _repsonse = _status.ToString();

            if(_userpostRelation == null){
                _userpostRelation = new UserPostRelation{
                    UserId = _UserId,
                    PostId = _PostId,
                    Liked = _status
                };
                _context.UserPostRelations.Add(_userpostRelation);
                ValueChangePostLike(_userpostRelation,_post,_user,_userpostRelation.Liked);
            }
            else {
                if(_userpostRelation.Liked == _status){
                    _context.UserPostRelations.Remove(_userpostRelation);
                    ValueChangePostLike(_userpostRelation,_post,_user,!_status);
                    _repsonse = "neutral";
                }
                else{
                    ValueChangePostLike(_userpostRelation,_post,_user,_status);
                    ValueChangePostLike(_userpostRelation,_post,_user,_status);
                }
                _userpostRelation.Liked = !_userpostRelation.Liked;
            }

            _context.SaveChanges();

            return _repsonse;
        }
        public void ValueChangePostLike(UserPostRelation _upr , Post _p , User _u, bool _status){
            int _s = _status ? 1:-1;
            foreach(PostTag _pt in _p.Tags){
                UserTag _ut = _context.UserTags.FirstOrDefault(x=>x.UserId == _u.Id && x.TagId == _pt.TagId);
                if(_ut == null){
                    _context.UserTags.Add(new UserTag{
                        Likes = _s,
                        UserId = _u.Id,
                        TagId = _pt.TagId
                    });
                }
                else{
                    _ut.Likes = _ut.Likes + _s;
                    if(_ut.Likes == 0){
                        _context.UserTags.Remove(_ut);
                    }
                }
                _context.SaveChanges();
            }
            _p.Likes = _p.Likes + _s;
            _context.SaveChanges();
        }
        public string ToggleUserCommentRelation(Guid _UserId, Guid _CommentId, bool _status){
            
            UserCommentRelation _userCommentRelation = _context.UserCommentRelations.FirstOrDefault(x=>x.CommentId == _CommentId && x.UserId == _UserId);
            Comment _comment = _context.Comments.FirstOrDefault(x => x.Id == _CommentId);

            string _repsonse = _status.ToString();
            int _s = _status ? 1:-1;

            if(_userCommentRelation == null){
                _userCommentRelation = new UserCommentRelation{
                    UserId = _UserId,
                    CommentId = _CommentId,
                    Liked = _status
                };
                _context.UserCommentRelations.Add(_userCommentRelation);
            }
            else {
                if(_userCommentRelation.Liked == _status){
                    _context.UserCommentRelations.Remove(_userCommentRelation);
                    _comment.Likes = _comment.Likes + _s;
                    _repsonse = "neutral";
                }
                _userCommentRelation.Liked = !_userCommentRelation.Liked;
            }

            _comment.Likes = _comment.Likes + _s;
            _context.SaveChanges();

            return _repsonse;
        }
    }
}