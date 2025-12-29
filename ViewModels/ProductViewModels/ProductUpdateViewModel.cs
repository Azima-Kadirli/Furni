using Microsoft.Identity.Client;

namespace Furni.ViewModels.ProductViewModels
{
    public class ProductUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageName { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
