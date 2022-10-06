using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models{
    public class Post : BaseModel{
        public string HeadLine {get; set;}
        public string Content {get; set;}
        public int Likes {get; set;}

        public Guid UserGuid {get; set;}
        [ForeignKey("UserGuid")]
        public User Poster {get; set;}

        public ICollection<PostTag> Tags  {get; set;}
        public ICollection<Comment> Comments  {get; set;}
        public ICollection<UserPostRelation> UsersLiked  {get; set;}
    }
}