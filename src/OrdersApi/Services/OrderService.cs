using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Dtos;
using OrdersApi.Models;

namespace OrdersApi.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<Order>> GetOrdersAsync(OrderFilterDto filter)
    {
        var query = _db.Orders.AsNoTracking().AsQueryable();

        if (filter.Id.HasValue)
        {
            query = query.Where(o => o.Id == filter.Id.Value);
        }

        if (filter.AmountFrom.HasValue)
        {
            query = query.Where(o => o.Amount >= filter.AmountFrom.Value);
        }

        if (filter.AmountTo.HasValue)
        {
            query = query.Where(o => o.Amount <= filter.AmountTo.Value);
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(o => o.CreatedAt >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(o => o.CreatedAt <= filter.CreatedTo.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(o => o.Status == filter.Status.Value);
        }

        if (filter.ClientId.HasValue)
        {
            query = query.Where(o => o.ClientId == filter.ClientId.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(o => o.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Order>
        {
            Items = items,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<ServiceResult<Order>> CreateOrderAsync(OrderCreateDto dto)
    {
        var clientExists = await _db.Clients.AnyAsync(c => c.Id == dto.ClientId);
        if (!clientExists)
        {
            return ServiceResult<Order>.Invalid($"Клиент с id={dto.ClientId} не найден.");
        }

        var order = new Order
        {
            Amount = dto.Amount,
            CreatedAt = DateTime.SpecifyKind(dto.CreatedAt, DateTimeKind.Unspecified),
            Status = dto.Status,
            ClientId = dto.ClientId
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return ServiceResult<Order>.Ok(order);
    }

    public async Task<ServiceResult<Order>> UpdateOrderAsync(int id, OrderUpdateDto dto)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order is null)
        {
            return ServiceResult<Order>.NotFound();
        }

        var clientExists = await _db.Clients.AnyAsync(c => c.Id == dto.ClientId);
        if (!clientExists)
        {
            return ServiceResult<Order>.Invalid($"Клиент с id={dto.ClientId} не найден.");
        }

        order.Amount = dto.Amount;
        order.CreatedAt = DateTime.SpecifyKind(dto.CreatedAt, DateTimeKind.Unspecified);
        order.Status = dto.Status;
        order.ClientId = dto.ClientId;

        await _db.SaveChangesAsync();
        return ServiceResult<Order>.Ok(order);
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order is null)
        {
            return false;
        }

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
        return true;
    }
}
