using Furni.Models.Common;

namespace Furni.Models;

public class EmployeeService:BaseEntity
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
}