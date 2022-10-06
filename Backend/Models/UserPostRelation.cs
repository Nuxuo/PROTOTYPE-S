using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class UserPostRelation :BaseModel_Id{
        public bool Liked {get; set;}
        public Guid UserGuid {get; set;}
        [ForeignKey("UserGuid")]
        public User User {get; set;}

        public Guid PostGuid {get; set;}
        [ForeignKey("PostGuid")]
        public Post Post {get; set;}
    }
}