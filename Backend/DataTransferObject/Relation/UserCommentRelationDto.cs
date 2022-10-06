using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class UserCommentRelationDto {  
        public Guid CommentGuid {get; set;}
        public bool Liked {get; set;}
    }
}