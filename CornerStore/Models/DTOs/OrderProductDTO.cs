using System.ComponentModel.DataAnnotations;

namespace CornerStore.Models.DTOs;

public class OrderProductDTO
{
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    public ProductDTO Product { get; set; }
    [Required]
    public int OrderId { get; set; }
    public OrderDTO Order { get; set; }
    [Required]
    public int Quantity { get; set; }
}