using System.ComponentModel.DataAnnotations;
using OrdersApi.Models;

namespace OrdersApi.Dtos;

public class OrderCreateDto
{
    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public int ClientId { get; set; }
}

public class OrderUpdateDto : OrderCreateDto
{
}
