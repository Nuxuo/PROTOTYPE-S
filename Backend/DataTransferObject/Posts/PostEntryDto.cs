using Models;
using DataTransferObject.Base;

namespace DataTransferObject{
    public class PostEntryDto{
        public Guid UserGuid {get; set;}
        public string Headline {get; set;}
        public string Content {get; set;}
        public ICollection<string> Tags  {get; set;}
    }
}