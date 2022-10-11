using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class UserPostRelation :BaseModel_Id{
        public bool Liked {get; set;}
        public Guid UserId {get; set;}
        [ForeignKey("UserId")]
        public User User {get; set;}

        public Guid PostId {get; set;}
        [ForeignKey("PostId")]
        public Post Post {get; set;}
    }
}