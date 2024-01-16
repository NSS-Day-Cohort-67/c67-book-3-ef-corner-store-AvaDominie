using System.ComponentModel.DataAnnotations;

namespace CornerStore.Models.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    [Required]
    public int CashierId { get; set; }
    public CashierDTO Cashier { get; set; }
    public List<OrderProductDTO> OrderProducts { get; set; }
    public decimal Total 
    {
        get
        {
             return OrderProducts?.Sum(op => op.Product.Price * op.Quantity) ?? 0m;
        }
    }
    public DateTime PaidOnDate { get; set; }
}