using Furni.Models;

namespace Furni.ViewModels.BlogViewModels
{
    public class BlogGetViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
        public string ImageName { get; set; }
        public List<string>TagNames { get; set; }
        public Employee Employee { get; set; }
    }
}
