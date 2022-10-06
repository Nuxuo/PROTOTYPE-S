using System.ComponentModel.DataAnnotations;

namespace Models{
    public class BaseModel : BaseModel_Id{ 
        
        public DateTime CreatedDate {get; set;}

        public DateTime UpdatedDate {get; set;}

        public BaseModel(){
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}