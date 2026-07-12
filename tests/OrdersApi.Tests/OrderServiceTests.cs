using OrdersApi.Dtos;
using OrdersApi.Models;
using OrdersApi.Services;
using Xunit;

namespace OrdersApi.Tests;

public class OrderServiceTests
{
    [Fact]
    public async Task GetOrders_FiltersByStatus()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new OrderService(db);

        var page = await service.GetOrdersAsync(new OrderFilterDto { Status = OrderStatus.Completed });

        Assert.Equal(1, page.TotalCount);
        Assert.All(page.Items, o => Assert.Equal(OrderStatus.Completed, o.Status));
    }

    [Fact]
    public async Task GetOrders_AppliesPagination()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new OrderService(db);

        var page = await service.GetOrdersAsync(new OrderFilterDto { Page = 2, PageSize = 2 });

        Assert.Equal(3, page.TotalCount);
        Assert.Single(page.Items);
        Assert.Equal(2, page.Page);
    }

    [Fact]
    public async Task GetOrders_FiltersByAmountRange()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new OrderService(db);

        var page = await service.GetOrdersAsync(new OrderFilterDto
        {
            AmountFrom = 200m,
            AmountTo = 300m
        });

        Assert.Equal(1, page.TotalCount);
        Assert.Equal(250m, page.Items.Single().Amount);
    }

    [Fact]
    public async Task CreateOrder_ReturnsValidationError_WhenClientDoesNotExist()
    {
        using var db = TestHelpers.CreateContext();
        var service = new OrderService(db);

        var result = await service.CreateOrderAsync(new OrderCreateDto
        {
            Amount = 100m,
            CreatedAt = DateTime.Now,
            Status = OrderStatus.NotProcessed,
            ClientId = 12345
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(ServiceErrorType.ValidationError, result.ErrorType);
        Assert.Empty(db.Orders);
    }

    [Fact]
    public async Task UpdateOrder_ChangesStatus()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new OrderService(db);
        var order = db.Orders.First(o => o.Status == OrderStatus.NotProcessed);

        var result = await service.UpdateOrderAsync(order.Id, new OrderUpdateDto
        {
            Amount = order.Amount,
            CreatedAt = order.CreatedAt,
            Status = OrderStatus.Completed,
            ClientId = order.ClientId
        });

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Completed, result.Value!.Status);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new OrderService(db);
        var clientId = db.Clients.First().Id;

        var result = await service.UpdateOrderAsync(999, new OrderUpdateDto
        {
            Amount = 1m,
            CreatedAt = DateTime.Now,
            Status = OrderStatus.Completed,
            ClientId = clientId
        });

        Assert.Equal(ServiceErrorType.NotFound, result.ErrorType);
    }

    [Fact]
    public async Task DeleteOrder_RemovesOrder()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new OrderService(db);
        var orderId = db.Orders.First().Id;

        var deleted = await service.DeleteOrderAsync(orderId);

        Assert.True(deleted);
        Assert.Null(await db.Orders.FindAsync(orderId));
    }
}
