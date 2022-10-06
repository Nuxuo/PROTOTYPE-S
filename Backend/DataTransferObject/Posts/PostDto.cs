using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class PostDto : BaseDto{
        public string HeadLine {get; set;}
        public string Content {get; set;}
        public int Likes {get; set;}
        public UserSimpleDto Poster {get; set;}
        public ICollection<CommentSimpleUserDto> Comments  {get; set;}
        public ICollection<TagDto> Tags  {get; set;}
    }
}