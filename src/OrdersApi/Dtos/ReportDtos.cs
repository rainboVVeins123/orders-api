namespace OrdersApi.Dtos;

/// <summary>Сумма выполненных заказов клиента, сделанных в его день рождения.</summary>
public class BirthdayOrdersTotalDto
{
    public int ClientId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }
}

/// <summary>Средний чек за час по выполненным заказам.</summary>
public class HourlyAverageCheckDto
{
    public int Hour { get; set; }

    public decimal AverageCheck { get; set; }
}
