using Microsoft.EntityFrameworkCore;
using DataTransferObject;
using AutoMapper;
using Context;
using Models;

namespace Repositories{
    public class TestingRepos : BaseRepos, ITestingRepos{              
        public TestingRepos(DatabaseContext context){
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public async Task<bool> CreatePosts(int ammount){
            List<User> _users = await _context.Users.ToListAsync(); int _userC = _users.Count();
            List<Tag> _tags = await _context.Tags.ToListAsync();

            Random rand = new Random();

            for(int i = 0; i < ammount; i ++){
                Post _post = new Post{
                    UserId = _users[rand.Next(1,_userC)].Id,
                    HeadLine = "test"+i,
                    Content = "test"+i
                };
                await _context.Posts.AddAsync(_post);
                await _context.SaveChangesAsync();

                for(int _tagIndex = 0; _tagIndex < rand.Next(1,4); _tagIndex ++){
                    _context.PostTags.Add(new PostTag{
                        PostId = _post.Id,
                        TagId = _tags[rand.Next(1,_tags.Count())].Id
                    });

                    _context.SaveChanges();
                }
            
            }

            return true;
        }
        public async Task<bool> CreateComments(int ammount){
            List<User> _users = await _context.Users.ToListAsync(); int _userC = _users.Count();
            List<Post> _posts = await _context.Posts.ToListAsync(); int _postC = _posts.Count();
            Random rand = new Random();

            for(int i = 0; i < ammount; i ++){
                Comment _comment = new Comment{
                    Likes = 0,
                    Content = "test"+i,
                    PostId = _posts[rand.Next(0,_postC)].Id,
                    UserId = _users[rand.Next(0,_userC)].Id
                };

                _context.Add(_comment);
                _context.SaveChanges();
            }

            return true;

        }
        public async Task<bool> UserRelationPosts(int ammount){
            List<User> _users = await _context.Users.ToListAsync(); int _userC = _users.Count();
            List<Post> _posts = await _context.Posts.ToListAsync(); int _postC = _posts.Count();
            Random rand = new Random();
            
            foreach(User _u in _users){
                for(int i = 0; i < ammount; i++){
                    ToggleUserPostRelation(_u.Id,_posts[rand.Next(0,_postC)].Id,rand.Next(2) == 1 ? true : false);
                }
            }

            
            return true;

        }
        public async Task<bool> UserRelationComments(int ammount){
            List<User> _users = await _context.Users.ToListAsync(); int _userC = _users.Count();
            List<Comment> _comments = await _context.Comments.ToListAsync(); int _commentsC = _comments.Count();
            Random rand = new Random();

            foreach(User _u in _users){
                for(int i = 0; i < ammount; i++){
                    ToggleUserCommentRelation(_u.Id,_comments[rand.Next(0,_commentsC)].Id,rand.Next(2) == 1 ? true : false);
                }
            }
            
            return true;

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
        public void ValueChangePostLike(UserPostRelation _upr , Post _p , User _u, bool _l){
            int _s = _l ? 1:-1;
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
        public async Task<bool> TruncateData(){
            _context.PostTags.RemoveRange(await _context.PostTags.ToListAsync());
            await _context.SaveChangesAsync();     
            _context.UserPostRelations.RemoveRange(await _context.UserPostRelations.ToListAsync());
            await _context.SaveChangesAsync();     
            _context.UserCommentRelations.RemoveRange(await _context.UserCommentRelations.ToListAsync());
            await _context.SaveChangesAsync();    
            _context.Comments.RemoveRange(await _context.Comments.ToListAsync());
            await _context.SaveChangesAsync();   
            _context.Posts.RemoveRange(await _context.Posts.ToListAsync());
            await _context.SaveChangesAsync(); 
            _context.UserTags.RemoveRange(await _context.UserTags.ToListAsync());
            await _context.SaveChangesAsync();  
            return true;
        }
    }
}