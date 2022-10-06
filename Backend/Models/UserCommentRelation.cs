using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class UserCommentRelation : BaseModel_Id{
        public bool Liked {get; set;}
        public Guid UserGuid {get; set;}
        [ForeignKey("UserGuid")]
        public User User {get; set;}

        public Guid CommentGuid {get; set;}
        [ForeignKey("CommentGuid")]
        public Comment Comment {get; set;}
    }
}