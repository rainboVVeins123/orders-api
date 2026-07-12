using Microsoft.AspNetCore.Mvc;
using OrdersApi.Dtos;
using OrdersApi.Models;
using OrdersApi.Services;

namespace OrdersApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orders;

    public OrdersController(IOrderService orders)
    {
        _orders = orders;
    }

    /// <summary>Получение заказов с фильтрацией по всем полям и постраничным просмотром.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<Order>>> GetOrders([FromQuery] OrderFilterDto filter)
    {
        return Ok(await _orders.GetOrdersAsync(filter));
    }

    /// <summary>Получение заказа по идентификатору.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _orders.GetOrderByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    /// <summary>Создание заказа.</summary>
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderCreateDto dto)
    {
        var result = await _orders.CreateOrderAsync(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetOrder), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>Редактирование заказа.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Order>> UpdateOrder(int id, OrderUpdateDto dto)
    {
        var result = await _orders.UpdateOrderAsync(id, dto);

        return result.ErrorType switch
        {
            ServiceErrorType.NotFound => NotFound(),
            ServiceErrorType.ValidationError => BadRequest(result.ErrorMessage),
            _ => Ok(result.Value)
        };
    }

    /// <summary>Удаление заказа.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var deleted = await _orders.DeleteOrderAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
