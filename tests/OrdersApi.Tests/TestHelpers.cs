using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Models;

namespace OrdersApi.Tests;

public static class TestHelpers
{
    /// <summary>Создает изолированный контекст БД в памяти для одного теста.</summary>
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    /// <summary>Заполняет контекст тестовыми данными: 2 клиента и 3 заказа.</summary>
    public static void Seed(AppDbContext db)
    {
        var ivanov = new Client
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateOnly(2000, 5, 10)
        };
        var petrov = new Client
        {
            FirstName = "Петр",
            LastName = "Петров",
            BirthDate = new DateOnly(1999, 8, 21)
        };

        db.Clients.AddRange(ivanov, petrov);
        db.SaveChanges();

        db.Orders.AddRange(
            new Order
            {
                Amount = 100m,
                CreatedAt = new DateTime(2026, 6, 1, 10, 0, 0),
                Status = OrderStatus.Completed,
                ClientId = ivanov.Id
            },
            new Order
            {
                Amount = 250m,
                CreatedAt = new DateTime(2026, 6, 2, 12, 0, 0),
                Status = OrderStatus.NotProcessed,
                ClientId = ivanov.Id
            },
            new Order
            {
                Amount = 400m,
                CreatedAt = new DateTime(2026, 6, 3, 15, 0, 0),
                Status = OrderStatus.Cancelled,
                ClientId = petrov.Id
            });
        db.SaveChanges();
    }
}
