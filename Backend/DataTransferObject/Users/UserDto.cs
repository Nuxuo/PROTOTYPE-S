using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class UserDto : BaseDto{  
        public string Firstname {get; set;}
        public string Lastname {get; set;}
        public string Email {get; set;}
        // public ICollection<PostSimpleDto> Posts  {get; set;}
        // public ICollection<CommentSimplePostDto> Comments  {get; set;}
        // public ICollection<TagLikesDto> Tags  {get; set;}
        // public ICollection<UserPostRelationDto> UserPostRelation  {get; set;}
        // public ICollection<UserCommentRelationDto> UserCommentRelation  {get; set;}
    }
}