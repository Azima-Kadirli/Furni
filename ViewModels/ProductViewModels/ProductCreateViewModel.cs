using System.ComponentModel.DataAnnotations.Schema;

namespace Furni.ViewModels.ProductViewModels
{
    public class ProductCreateViewModel
    {

        public string Title { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
