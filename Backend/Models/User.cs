using System.ComponentModel.DataAnnotations;
using DataTransferObject;

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

        public User(){}
        public User(UserEntryDto _x){
            this.Firstname = _x.Firstname;
            this.Lastname = _x.Lastname;
            this.Email = _x.Email;
            this.Password = _x.Password;
        }

    }
}