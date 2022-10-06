using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class UserPostRelationDto {  
        public Guid PostGuid {get; set;}
        public bool Liked {get; set;}
    }
}