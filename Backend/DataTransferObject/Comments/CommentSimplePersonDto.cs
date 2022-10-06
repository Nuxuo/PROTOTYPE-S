using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class CommentSimpleUserDto : BaseSimpleDto{
        public int Likes {get; set;}
        public string Content {get; set;}
        public Guid UserGuid {get; set;}
    }
}