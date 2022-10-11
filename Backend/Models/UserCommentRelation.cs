using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class UserCommentRelation : BaseModel_Id{
        public bool Liked {get; set;}
        public Guid UserId {get; set;}
        [ForeignKey("UserId")]
        public User User {get; set;}

        public Guid CommentId {get; set;}
        [ForeignKey("CommentId")]
        public Comment Comment {get; set;}
    }
}