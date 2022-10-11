using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class CommentSimplePostDto : BaseSimpleDto{
        public int Likes {get; set;}
        public string Content {get; set;}
        public Guid PostId {get; set;}
    }
}