namespace Furni.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
