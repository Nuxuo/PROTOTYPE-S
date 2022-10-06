using Microsoft.EntityFrameworkCore;
using DataTransferObject;
using Context;
using Models;

namespace Repositories{
    public class CommentRepos : BaseRepos , ICommentRepos{

        public CommentRepos(DatabaseContext context){
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public Comment GetCommentById(Guid _guid){
            return _context.Comments.Where(x=>!x.SoftDeleted).FirstOrDefault(x=>x.guId == _guid);
        }

        // POST
        public Comment CreateComment(CommentEntryDto _input){
            if(_context.Users.Where(x=>!x.SoftDeleted).FirstOrDefault(x=>x.guId == _input.UserGuid)==null)
                return null;

            Comment _comment = new Comment{
                Likes = 0,
                Content = _input.Content,
                PostGuid = _input.PostGuid,
                UserGuid = _input.UserGuid
            };

            _context.Add(_comment);
            _context.SaveChanges();
            
            return _comment;
        }


        // PUT
        public Comment UpdateComment(Guid _guid, CommentEntryDto _input){
            var _comment = _context.Comments.Find(_guid);

            _comment.Content = _input.Content;
            _comment.UpdatedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return _comment;
        }

    }
}