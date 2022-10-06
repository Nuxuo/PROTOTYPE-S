using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models{
    public class PostTag : BaseModel_Id {
        public Guid PostGuid {get; set;}
        [ForeignKey("PostGuid")]
        public Post Post {get; set;}

        public Guid TagGuid {get; set;}
        [ForeignKey("TagGuid")]
        public Tag Tag {get; set;}
    }
}