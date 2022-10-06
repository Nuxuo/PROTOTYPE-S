using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class CommentEntryDto{
        public Guid UserGuid {get; set;}
        public Guid PostGuid {get; set;}
        public string Content {get; set;}
    }
}