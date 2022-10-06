using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class UserTag :BaseModel_Id {
        public int Likes {get; set;}

        public Guid UserGuid {get; set;}
        [ForeignKey("UserGuid")]
        public User User {get; set;}

        public Guid TagGuid {get; set;}
        [ForeignKey("TagGuid")]
        public Tag Tag {get; set;}
    }
}