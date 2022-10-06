using System.ComponentModel.DataAnnotations;

namespace Models{
    public class BaseModel_Id{
        [Key]
        public Guid guId {get; set;}
    }
}