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
            .FirstOrDefault(x => x.guId == id);
        }

        public IEnumerable<UserPostRelation> GetUsersUserPostRelation(Guid _guid){
            return _context.UserPostRelations.Where(x=>x.UserGuid == _guid).ToList();
        }

        public IEnumerable<UserCommentRelation> GetUsersUserCommentRelation(Guid _guid){
            return _context.UserCommentRelations.Where(x=>x.UserGuid == _guid).ToList();
        }

        public IEnumerable<Comment> GetUsersComments(Guid _guid){
            return _context.Comments.Where(x => x.UserGuid == _guid).ToList();
        }
        public IEnumerable<Post> GetUsersPosts(Guid _guid){
            return _context.Posts
                .Include(x=>x.Tags)
                    .ThenInclude(x=>x.Tag)
                .Where(x => x.UserGuid == _guid)
            .ToList();
        }
        public IEnumerable<UserTag> GetUsersTags(Guid _guid){
            return _context.UserTags
                .Include(x=>x.Tag)
                    .Where(x => x.UserGuid == _guid).ToList();
        }

        public IEnumerable<PostRatingDto> GetUsersTargetedPosts(Guid _guid, int ammount){
            List<PostRatingDto> _returnList = new List<PostRatingDto>();

            var _Posts = _context.Posts
                .Include(x=>x.Tags)
                    .ThenInclude(x=>x.Tag)
                .OrderBy(x=>x.CreatedDate)
                .Take(100);

            var _UserRelation = GetUsersUserPostRelation(_guid);
            var _UserTags = _context.UserTags.Where(x=>x.UserGuid == _guid).ToList();

            foreach (Post _p in _Posts){
                if(_UserRelation.FirstOrDefault(x=>x.PostGuid ==_p.guId) == null){
                    int _rating = 0;
                    foreach (PostTag _pt in _p.Tags){
                        UserTag _userTag = _UserTags.FirstOrDefault(x=>x.TagGuid == _pt.TagGuid);
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


        // POST
        public User CreateUser(UserEntryDto _input){
            User _User = new User{
                Firstname = _input.Firstname,
                Lastname = _input.Lastname,
                Email = _input.Email,
                Password = _input.Password
            };

            _context.Users.Add(_User);
            _context.SaveChanges();
            
            return _User;
        }


        // PUT
        public User UpdateUser(Guid _guid, UserEntryDto _input){
            var _User = _context.Users.Find(_guid);

            _User.Firstname = _input.Firstname;
            _User.Lastname = _input.Lastname;
            _User.Email = _input.Email;           
            _User.Password = _input.Password; 

            _context.SaveChanges();
            return _User;
        }

        public string ToggleUserPostRelation(Guid _UserGuid, Guid _PostGuid, bool _status){
            UserPostRelation _userpostRelation = _context.UserPostRelations.FirstOrDefault(x=>x.PostGuid == _PostGuid && x.UserGuid == _UserGuid);
            Post _post = _context.Posts.Include(x=>x.Tags).FirstOrDefault(x => x.guId == _PostGuid);
            User _user = _context.Users.Include(x=>x.Tags).Include(x=>x.UserPostRelation).FirstOrDefault(x => x.guId == _UserGuid);
            string _repsonse = _status.ToString();

            if(_userpostRelation == null){
                _userpostRelation = new UserPostRelation{
                    UserGuid = _UserGuid,
                    PostGuid = _PostGuid,
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

        public void ValueChangePostLike(UserPostRelation _upr , Post _p , User _u, bool _l){
            int _s = _l ? 1:-1;
            foreach(PostTag _pt in _p.Tags){
                UserTag _ut = _context.UserTags.FirstOrDefault(x=>x.UserGuid == _u.guId && x.TagGuid == _pt.TagGuid);
                if(_ut == null){
                    _context.UserTags.Add(new UserTag{
                        Likes = _s,
                        UserGuid = _u.guId,
                        TagGuid = _pt.TagGuid
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

        public string ToggleUserCommentRelation(Guid _UserGuid, Guid _CommentGuid, bool _status){
            
            UserCommentRelation _userCommentRelation = _context.UserCommentRelations.FirstOrDefault(x=>x.CommentGuid == _CommentGuid && x.UserGuid == _UserGuid);
            Comment _comment = _context.Comments.FirstOrDefault(x => x.guId == _CommentGuid);

            string _repsonse = _status.ToString();
            int _s = _status ? 1:-1;

            if(_userCommentRelation == null){
                _userCommentRelation = new UserCommentRelation{
                    UserGuid = _UserGuid,
                    CommentGuid = _CommentGuid,
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