using Microsoft.EntityFrameworkCore;
using DataTransferObject;
using AutoMapper;
using Context;
using Models;

namespace Repositories{
    public class BaseRepos : IBaseRepos{
        protected DatabaseContext _context;
        protected IMapper _mapper;


        public async Task<bool> UserExists(Guid _Id){
            return await _context.Users.Where(x=>!x.SoftDeleted).FirstOrDefaultAsync(x=>x.Id == _Id) != null ? true : false;
        }
        
        public async Task<bool> PostExists(Guid _Id){
            return await _context.Posts.FirstOrDefaultAsync(x=>x.Id == _Id) != null ? true : false;
        }

        public async Task<bool> CommentExists(Guid _Id){
            return await _context.Comments.Where(x=>!x.SoftDeleted).FirstOrDefaultAsync(x=>x.Id == _Id) != null ? true : false;
        }


        public async Task<bool> SoftDeleteCommentById(Guid _Id){
            if(await CommentExists(_Id)){
                var _comment = await _context.Comments.FirstOrDefaultAsync(x=>x.Id == _Id);
                _comment.SoftDeleted = true;
                _context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
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


        public async Task<bool> HardDeleteCommentById(Guid _Id){
            var _c = await _context.Comments.FirstOrDefaultAsync(x=>x.Id == _Id);
            var _userCommentRelations = await _context.UserCommentRelations.Where(x=>x.CommentId == _Id).ToListAsync();
            foreach(UserCommentRelation _upr in _userCommentRelations){
                _context.UserCommentRelations.Remove(_upr);
                _context.SaveChanges();
            }
            
            _context.Comments.Remove(_c);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> HardDeletePostById(Guid _Id){
            var _p = _context.Posts.FirstOrDefault(x=>x.Id == _Id);

            var _postTag = await _context.PostTags.Where(x=>x.PostId == _p.Id).ToListAsync();
            foreach(PostTag _t in _postTag){
                _context.PostTags.Remove(_t);
                _context.SaveChanges();
            }    

            var _userRelations = await _context.UserPostRelations.Where(x=>x.PostId == _p.Id).ToListAsync();
            foreach(UserPostRelation _upr in _userRelations){
                _context.UserPostRelations.Remove(_upr);
                _context.SaveChanges();
            }        
            
            var _comments = await _context.Comments.Where(x=>x.PostId == _p.Id).ToListAsync();
            if (_comments == null) { return false; }
            foreach(Comment _c in _comments){
                await HardDeleteCommentById(_c.Id);
            }


            _context.SaveChanges();
            _context.Posts.Remove(_p);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> HardDeleteUserById(Guid _Id){
            var _u = _context.Users.FirstOrDefault(x=>x.Id == _Id);
            if (_u == null) { return false; }
            Console.WriteLine("User found");

            var _userTags = await _context.UserTags.Where(x=>x.UserId == _Id).ToListAsync();
            foreach(UserTag _ut in _userTags){
                _context.UserTags.Remove(_ut);
                _context.SaveChanges();
            }    

            var _posts = await _context.Posts.Where(x=>x.UserId == _Id).ToListAsync();
            if (_posts == null) { return false; }
            foreach(Post _p in _posts){
                await HardDeletePostById(_p.Id);
            }

            _context.Users.Remove(_u);
            _context.SaveChanges();
            return true;
        }

    }
}