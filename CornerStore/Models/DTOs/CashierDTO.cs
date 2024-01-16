using System.ComponentModel.DataAnnotations;

namespace CornerStore.Models.DTOs;

public class CashierDTO
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
}