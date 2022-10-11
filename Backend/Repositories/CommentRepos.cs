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
        public Comment CreateComment(CommentEntryDto _entry){
            if(_context.Users.Where(x=>!x.SoftDeleted).FirstOrDefault(x=>x.Id == _entry.UserId)==null)
                return null;

            Comment _comment = new Comment(_entry);

            _context.Add(_comment);
            _context.SaveChanges();
            
            return _comment;
        }
        public Comment UpdateComment(Guid _Id, CommentEntryDto _entry){
            var _comment = _context.Comments.Find(_Id);

            _comment.Content = _entry.Content;
            _comment.UpdatedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return _comment;
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

    }
}