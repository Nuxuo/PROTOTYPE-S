using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class UserCommentRelationDto {  
        public Guid CommentId {get; set;}
        public bool Liked {get; set;}
    }
}