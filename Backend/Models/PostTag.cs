using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class PostTag : BaseModel_Id {
        public Guid PostId {get; set;}
        [ForeignKey("PostId")]
        public Post Post {get; set;}

        public Guid TagId {get; set;}
        [ForeignKey("TagId")]
        public Tag Tag {get; set;}
    }
}