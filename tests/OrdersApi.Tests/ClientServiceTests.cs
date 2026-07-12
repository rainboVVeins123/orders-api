using OrdersApi.Dtos;
using OrdersApi.Services;
using Xunit;

namespace OrdersApi.Tests;

public class ClientServiceTests
{
    [Fact]
    public async Task CreateClient_AddsClientToDatabase()
    {
        using var db = TestHelpers.CreateContext();
        var service = new ClientService(db);

        var client = await service.CreateClientAsync(new ClientCreateDto
        {
            FirstName = "Анна",
            LastName = "Сидорова",
            BirthDate = new DateOnly(2001, 1, 15)
        });

        Assert.True(client.Id > 0);
        Assert.Single(db.Clients);
        Assert.Equal("Сидорова", db.Clients.Single().LastName);
    }

    [Fact]
    public async Task GetClients_FiltersByLastName()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new ClientService(db);

        var page = await service.GetClientsAsync(new ClientFilterDto { LastName = "Петров" });

        Assert.Equal(1, page.TotalCount);
        Assert.Equal("Петров", page.Items.Single().LastName);
    }

    [Fact]
    public async Task GetClients_FiltersByBirthDateRange()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new ClientService(db);

        var page = await service.GetClientsAsync(new ClientFilterDto
        {
            BirthDateFrom = new DateOnly(2000, 1, 1)
        });

        Assert.Equal(1, page.TotalCount);
        Assert.Equal("Иванов", page.Items.Single().LastName);
    }

    [Fact]
    public async Task UpdateClient_ReturnsNull_WhenClientDoesNotExist()
    {
        using var db = TestHelpers.CreateContext();
        var service = new ClientService(db);

        var result = await service.UpdateClientAsync(999, new ClientUpdateDto
        {
            FirstName = "Имя",
            LastName = "Фамилия",
            BirthDate = new DateOnly(2000, 1, 1)
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteClient_RemovesClient()
    {
        using var db = TestHelpers.CreateContext();
        TestHelpers.Seed(db);
        var service = new ClientService(db);
        var clientId = db.Clients.First().Id;

        var deleted = await service.DeleteClientAsync(clientId);

        Assert.True(deleted);
        Assert.Null(await db.Clients.FindAsync(clientId));
    }
}
