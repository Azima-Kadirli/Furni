using System.ComponentModel.DataAnnotations.Schema;
using Furni.Models;

namespace Furni.ViewModels.BlogViewModels
{
    public class BlogCreateViewModel
    {

        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
        [NotMapped]
        public IFormFile ImageName { get; set; }
        public int EmployeeId { get; set; }
        public List<int>TagIds { get; set; }
    }
}
