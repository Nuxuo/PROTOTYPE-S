using Microsoft.EntityFrameworkCore;
using DataTransferObject;
using AutoMapper;
using Context;
using Models;

namespace Repositories{
    public class BaseRepos : IBaseRepos{
        protected DatabaseContext _context;
        protected IMapper _mapper;


        public bool UserExists(Guid _guid){
            return _context.Users.Where(x=>!x.SoftDeleted).FirstOrDefault(x=>x.guId == _guid) != null ? true : false;
        }
        
        public bool PostExists(Guid _guid){
            return _context.Posts.Find(_guid) != null ? true : false;
        }

        public bool CommentExists(Guid _guid){
            return _context.Comments.Where(x=>!x.SoftDeleted).FirstOrDefault(x=>x.guId == _guid) != null ? true : false;
        }


        public bool SoftDeleteCommentById(Guid _guid){
            if(CommentExists(_guid)){
                var _comment = _context.Comments.Find(_guid);
                _comment.SoftDeleted = true;
                _context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }
        public bool SoftDeleteUserById(Guid _guid){
            if(UserExists(_guid)){
                var _User = _context.Users.Find(_guid);
                _User.SoftDeleted = true;
                _context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }


        public bool HardDeleteCommentById(Guid _guid){
            var _comment = _context.Comments.Find(_guid);
            if (_comment == null) { return false; }

            var _userCommentRelations = _context.UserCommentRelations.Where(x=>x.CommentGuid == _guid);
            foreach(UserCommentRelation _upr in _userCommentRelations){
                _context.UserCommentRelations.Remove(_upr);
            }
            _context.SaveChanges();
            _context.Comments.Remove(_comment);
            _context.SaveChanges();

            return true;
        }

        public bool HardDeletePostById(Guid _guid){
            var _post = _context.Posts.Find(_guid);
            if (_post == null) { return false; }

            var _postTag = _context.PostTags.Where(x=>x.PostGuid == _guid);
            foreach(PostTag _t in _postTag){
                _context.PostTags.Remove(_t);
            }            
            
            var _comments = _context.Comments.Where(x=>x.PostGuid == _guid);
            foreach(Comment _c in _comments){
                HardDeleteCommentById(_c.guId);
            }


            _context.SaveChanges();
            _context.Posts.Remove(_post);
            _context.SaveChanges();

            return true;
        }

        public bool HardDeleteUserById(Guid _guid){
            var _User = _context.Users.Find(_guid);
            if (_User == null) { return false; }

            var _posts = _context.Posts.Where(x=>x.UserGuid == _guid);
            foreach(Post _p in _posts){
                HardDeletePostById(_p.guId);
            }

            _context.Users.Remove(_User);
            _context.SaveChanges();
            return true;
        }

    }
}