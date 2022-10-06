using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class PostSimpleDto : BaseSimpleDto{
        public string HeadLine {get; set;}
        public string Content {get; set;}
        public int Likes {get; set;}
        public ICollection<TagDto> Tags  {get; set;}
    }
}