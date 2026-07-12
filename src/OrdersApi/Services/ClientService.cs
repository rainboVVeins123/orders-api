using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Dtos;
using OrdersApi.Models;

namespace OrdersApi.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _db;

    public ClientService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<Client>> GetClientsAsync(ClientFilterDto filter)
    {
        var query = _db.Clients.AsNoTracking().AsQueryable();

        if (filter.Id.HasValue)
        {
            query = query.Where(c => c.Id == filter.Id.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
        {
            query = query.Where(c => c.FirstName.ToLower().Contains(filter.FirstName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.LastName))
        {
            query = query.Where(c => c.LastName.ToLower().Contains(filter.LastName.ToLower()));
        }

        if (filter.BirthDateFrom.HasValue)
        {
            query = query.Where(c => c.BirthDate >= filter.BirthDateFrom.Value);
        }

        if (filter.BirthDateTo.HasValue)
        {
            query = query.Where(c => c.BirthDate <= filter.BirthDateTo.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Client>
        {
            Items = items,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client> CreateClientAsync(ClientCreateDto dto)
    {
        var client = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate
        };

        _db.Clients.Add(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> UpdateClientAsync(int id, ClientUpdateDto dto)
    {
        var client = await _db.Clients.FindAsync(id);
        if (client is null)
        {
            return null;
        }

        client.FirstName = dto.FirstName;
        client.LastName = dto.LastName;
        client.BirthDate = dto.BirthDate;

        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var client = await _db.Clients.FindAsync(id);
        if (client is null)
        {
            return false;
        }

        _db.Clients.Remove(client);
        await _db.SaveChangesAsync();
        return true;
    }
}
