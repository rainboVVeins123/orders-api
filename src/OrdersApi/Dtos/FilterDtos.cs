using System.ComponentModel.DataAnnotations;
using OrdersApi.Models;

namespace OrdersApi.Dtos;

/// <summary>Параметры фильтрации и пагинации списка клиентов.</summary>
public class ClientFilterDto
{
    public int? Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDateFrom { get; set; }

    public DateOnly? BirthDateTo { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}

/// <summary>Параметры фильтрации и пагинации списка заказов.</summary>
public class OrderFilterDto
{
    public int? Id { get; set; }

    public decimal? AmountFrom { get; set; }

    public decimal? AmountTo { get; set; }

    public DateTime? CreatedFrom { get; set; }

    public DateTime? CreatedTo { get; set; }

    public OrderStatus? Status { get; set; }

    public int? ClientId { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}
