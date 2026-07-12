using OrdersApi.Dtos;
using OrdersApi.Models;

namespace OrdersApi.Services;

/// <summary>Бизнес-логика работы с заказами.</summary>
public interface IOrderService
{
    Task<PagedResult<Order>> GetOrdersAsync(OrderFilterDto filter);

    Task<Order?> GetOrderByIdAsync(int id);

    Task<ServiceResult<Order>> CreateOrderAsync(OrderCreateDto dto);

    Task<ServiceResult<Order>> UpdateOrderAsync(int id, OrderUpdateDto dto);

    /// <returns>false, если заказ не найден.</returns>
    Task<bool> DeleteOrderAsync(int id);
}
