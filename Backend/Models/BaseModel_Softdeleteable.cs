using System.ComponentModel.DataAnnotations;

namespace Models{
    public class BaseModel_Softdeleteable : BaseModel{
        public bool SoftDeleted {get; set;}

        public BaseModel_Softdeleteable(){
            SoftDeleted = false;
        }
    }
}