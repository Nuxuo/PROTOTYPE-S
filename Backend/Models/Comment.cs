using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models{
    public class Comment : BaseModel_Softdeleteable{
        public int Likes {get; set;}
        public string Content {get; set;}
        public Guid UserId {get; set;}
        public Guid PostId {get; set;}
        public ICollection<UserCommentRelation> UsersLiked  {get; set;}

    }
}