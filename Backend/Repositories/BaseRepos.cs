using Microsoft.EntityFrameworkCore;
using DataTransferObject;
using AutoMapper;
using Context;
using Models;

namespace Repositories{
    public class BaseRepos : IBaseRepos{
        protected DatabaseContext _context;
        protected IMapper _mapper;


        public async Task<bool> UserExists(Guid _guid){
            return await _context.Users.Where(x=>!x.SoftDeleted).FirstOrDefaultAsync(x=>x.guId == _guid) != null ? true : false;
        }
        
        public async Task<bool> PostExists(Guid _guid){
            return await _context.Posts.FirstOrDefaultAsync(x=>x.guId == _guid) != null ? true : false;
        }

        public async Task<bool> CommentExists(Guid _guid){
            return await _context.Comments.Where(x=>!x.SoftDeleted).FirstOrDefaultAsync(x=>x.guId == _guid) != null ? true : false;
        }


        public async Task<bool> SoftDeleteCommentById(Guid _guid){
            if(await CommentExists(_guid)){
                var _comment = await _context.Comments.FirstOrDefaultAsync(x=>x.guId == _guid);
                _comment.SoftDeleted = true;
                _context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }
        public async Task<bool> SoftDeleteUserById(Guid _guid){
            if(await UserExists(_guid)){
                var _User = await _context.Users.FirstOrDefaultAsync(x=>x.guId == _guid);
                _User.SoftDeleted = true;
                _context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }


        public async Task<bool> HardDeleteCommentById(Guid _guid){
            var _c = await _context.Comments.FirstOrDefaultAsync(x=>x.guId == _guid);
            var _userCommentRelations = await _context.UserCommentRelations.Where(x=>x.CommentGuid == _guid).ToListAsync();
            foreach(UserCommentRelation _upr in _userCommentRelations){
                _context.UserCommentRelations.Remove(_upr);
                _context.SaveChanges();
            }
            
            _context.Comments.Remove(_c);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> HardDeletePostById(Guid _guid){
            var _p = _context.Posts.FirstOrDefault(x=>x.guId == _guid);

            var _postTag = await _context.PostTags.Where(x=>x.PostGuid == _p.guId).ToListAsync();
            foreach(PostTag _t in _postTag){
                _context.PostTags.Remove(_t);
                _context.SaveChanges();
            }    

            var _userRelations = await _context.UserPostRelations.Where(x=>x.PostGuid == _p.guId).ToListAsync();
            foreach(UserPostRelation _upr in _userRelations){
                _context.UserPostRelations.Remove(_upr);
                _context.SaveChanges();
            }        
            
            var _comments = await _context.Comments.Where(x=>x.PostGuid == _p.guId).ToListAsync();
            if (_comments == null) { return false; }
            foreach(Comment _c in _comments){
                await HardDeleteCommentById(_c.guId);
            }


            _context.SaveChanges();
            _context.Posts.Remove(_p);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> HardDeleteUserById(Guid _guid){
            var _u = _context.Users.FirstOrDefault(x=>x.guId == _guid);
            if (_u == null) { return false; }
            Console.WriteLine("User found");

            var _userTags = await _context.UserTags.Where(x=>x.UserGuid == _guid).ToListAsync();
            foreach(UserTag _ut in _userTags){
                _context.UserTags.Remove(_ut);
                _context.SaveChanges();
            }    

            var _posts = await _context.Posts.Where(x=>x.UserGuid == _guid).ToListAsync();
            if (_posts == null) { return false; }
            foreach(Post _p in _posts){
                await HardDeletePostById(_p.guId);
            }

            _context.Users.Remove(_u);
            _context.SaveChanges();
            return true;
        }

    }
}