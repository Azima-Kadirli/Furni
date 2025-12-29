using Furni.Models.Common;

namespace Furni.Models
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
