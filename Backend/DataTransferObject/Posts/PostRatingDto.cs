using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class PostRatingDto : BaseSimpleDto{
        public int Rating {get; set;}
        public string HeadLine {get; set;}
        public string Content {get; set;}
        public int Likes {get; set;}
        public ICollection<TagDto> Tags  {get; set;}
    }
}