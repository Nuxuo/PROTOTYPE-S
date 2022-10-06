using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models{
    public class Tag : BaseModel_Id{
        public string Name {get; set;}
        public ICollection<PostTag> PostTags  {get; set;}
        public ICollection<UserTag> UserTags  {get; set;}
    }
}