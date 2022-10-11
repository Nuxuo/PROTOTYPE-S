using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataTransferObject;

namespace Models{
    public class Post : BaseModel{
        public string HeadLine {get; set;}
        public string Content {get; set;}
        public int Likes {get; set;}

        public Guid UserId {get; set;}
        [ForeignKey("UserId")]
        public User Poster {get; set;}

        public ICollection<PostTag> Tags  {get; set;}
        public ICollection<Comment> Comments  {get; set;}
        public ICollection<UserPostRelation> UsersLiked  {get; set;}

        public Post(){}
        public Post(PostEntryDto _x){
            this.Likes = 0;
            this.HeadLine = _x.Headline;
            this.Content = _x.Content;
            this.UserId = _x.UserId;
        }
    }
}