using Furni.Models.Common;

namespace Furni.Models;

public class Service:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageName  { get; set; }
    public string ImageUrl { get; set; }
}