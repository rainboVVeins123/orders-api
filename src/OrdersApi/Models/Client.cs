namespace OrdersApi.Models;

/// <summary>Клиент.</summary>
public class Client
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public List<Order> Orders { get; set; } = new();
}
