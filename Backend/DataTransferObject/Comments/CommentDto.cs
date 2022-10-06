using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class CommentDto : BaseDto{
        public int Likes {get; set;}
        public string Content {get; set;}
        public Guid UserGuid {get; set;}
        public Guid PostGuid {get; set;}
    }
}