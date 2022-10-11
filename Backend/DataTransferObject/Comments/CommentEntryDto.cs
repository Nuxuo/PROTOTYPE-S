using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class CommentEntryDto{
        public Guid UserId {get; set;}
        public Guid PostId {get; set;}
        public string Content {get; set;}
    }
}