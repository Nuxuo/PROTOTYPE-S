using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class UserEntryDto{
        public string Firstname {get; set;}
        public string Lastname {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
    }
}