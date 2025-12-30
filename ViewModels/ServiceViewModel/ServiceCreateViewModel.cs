using System.ComponentModel.DataAnnotations.Schema;

namespace Furni.ViewModels.ServiceViewModel;

public class ServiceCreateViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [NotMapped]
    public IFormFile Image  { get; set; }
    public string ImageUrl { get; set; }
}