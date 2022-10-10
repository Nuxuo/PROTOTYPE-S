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
            List<Tag> _tags = await _context.Tags.ToListAsync();
            List<User> _users = await _context.Users.ToListAsync();
            Random rand = new Random();

            for(int i = 0; i < ammount; i ++){
                Post _post = new Post{
                    UserGuid = _users[rand.Next(0,_users.Count())].guId,
                    HeadLine = "test"+i,
                    Content = "test"+i
                };
                await _context.Posts.AddAsync(_post);
                await _context.SaveChangesAsync();

                for(int _tagIndex = 0; _tagIndex < rand.Next(1,4); _tagIndex ++){
                    _context.PostTags.Add(new PostTag{
                        PostGuid = _post.guId,
                        TagGuid = _tags[rand.Next(1,_tags.Count())].guId
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
                    PostGuid = _posts[rand.Next(0,_postC)].guId,
                    UserGuid = _users[rand.Next(0,_userC)].guId
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
            
            for(int i = 0; i < ammount; i++){
                ToggleUserPostRelation(_users[rand.Next(0,_userC)].guId,_posts[rand.Next(0,_postC)].guId,rand.Next(2) == 1 ? true : false);
            }
            
            return true;

        }
        public async Task<bool> UserRelationComments(int ammount){
            List<User> _users = await _context.Users.ToListAsync(); int _userC = _users.Count();
            List<Comment> _comments = await _context.Comments.ToListAsync(); int _commentsC = _comments.Count();
            Random rand = new Random();
            
            for(int i = 0; i < ammount; i++){
                ToggleUserCommentRelation(_users[rand.Next(0,_userC)].guId,_comments[rand.Next(0,_commentsC)].guId,rand.Next(2) == 1 ? true : false);
            }
            
            return true;

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