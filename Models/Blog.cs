using Furni.Models.Common;

namespace Furni.Models
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
