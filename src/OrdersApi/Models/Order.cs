using System.Text.Json.Serialization;

namespace OrdersApi.Models;

/// <summary>Заказ.</summary>
public class Order
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public OrderStatus Status { get; set; }

    public int ClientId { get; set; }

    [JsonIgnore]
    public Client? Client { get; set; }
}
