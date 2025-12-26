namespace Furni.ViewModels.ProductViewModels
{
    public class ProductIndexViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
    }
}
