using System.ComponentModel.DataAnnotations;

namespace DataTransferObject.Base{
    public class BaseDto : IdBase{
        public DateTime CreatedDate {get; set;}
        public DateTime UpdatedDate {get; set;}
        public List<Link> Links {get; set;}
    }
}