using System.ComponentModel.DataAnnotations;

namespace Models{
    public class User : BaseModel_Softdeleteable{
        public string Firstname {get; set;}
        public string Lastname {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public ICollection<Post> Posts  {get; set;}
        public ICollection<Comment> Comments  {get; set;}
        public ICollection<UserTag> Tags  {get; set;}
        public ICollection<UserPostRelation> UserPostRelation  {get; set;}
        public ICollection<UserCommentRelation> UserCommentRelations  {get; set;}
    }
}