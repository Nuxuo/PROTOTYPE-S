using System.ComponentModel.DataAnnotations;

namespace DataTransferObject.Base{
    public class BaseSimpleDto : IdBase{
        public DateTime CreatedDate {get; set;}
    }
}