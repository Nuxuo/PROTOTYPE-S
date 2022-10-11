using Microsoft.EntityFrameworkCore;
using DataTransferObject;
using Context;
using Models;

namespace Repositories{
    public class CommentRepos : BaseRepos , ICommentRepos{

        public CommentRepos(DatabaseContext context){
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        public Comment GetCommentById(Guid _Id){
            return _context.Comments.Where(x=>!x.SoftDeleted).FirstOrDefault(x=>x.Id == _Id);
        }

        // POST
        public Comment CreateComment(CommentEntryDto _input){
            if(_context.Users.Where(x=>!x.SoftDeleted).FirstOrDefault(x=>x.Id == _input.UserId)==null)
                return null;

            Comment _comment = new Comment(_input);

            _context.Add(_comment);
            _context.SaveChanges();
            
            return _comment;
        }


        // PUT
        public Comment UpdateComment(Guid _Id, CommentEntryDto _input){
            var _comment = _context.Comments.Find(_Id);

            _comment.Content = _input.Content;
            _comment.UpdatedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return _comment;
        }

    }
}