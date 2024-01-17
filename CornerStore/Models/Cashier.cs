using System.ComponentModel.DataAnnotations;

namespace CornerStore.Models;

public class Cashier
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string FullName
    {
        get
        {
            string name = FirstName + " " + LastName;
            return name;
        }
    }
    public List<Order> Orders { get; set; }
}