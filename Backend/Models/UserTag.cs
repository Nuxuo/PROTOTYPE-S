using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class UserTag :BaseModel_Id {
        public int Likes {get; set;}

        public Guid UserId {get; set;}
        [ForeignKey("UserId")]
        public User User {get; set;}

        public Guid TagId {get; set;}
        [ForeignKey("TagId")]
        public Tag Tag {get; set;}
    }
}