using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class UserSimpleDto : BaseSimpleDto{
        public string Firstname {get; set;}
        public string Lastname {get; set;}
        public string Email {get; set;}
    }
}