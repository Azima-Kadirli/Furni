using Furni.Models;

namespace Furni.ViewModels.BlogViewModels
{
    public class BlogUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string? ImageName { get; set; }
        public string? ImageUrl { get; set; }
        public List<int>TagIds { get; set; }
    }
}
