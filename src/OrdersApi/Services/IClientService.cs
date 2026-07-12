using OrdersApi.Dtos;
using OrdersApi.Models;

namespace OrdersApi.Services;

/// <summary>Бизнес-логика работы с клиентами.</summary>
public interface IClientService
{
    Task<PagedResult<Client>> GetClientsAsync(ClientFilterDto filter);

    Task<Client?> GetClientByIdAsync(int id);

    Task<Client> CreateClientAsync(ClientCreateDto dto);

    /// <returns>Обновленный клиент или null, если клиент не найден.</returns>
    Task<Client?> UpdateClientAsync(int id, ClientUpdateDto dto);

    /// <returns>false, если клиент не найден.</returns>
    Task<bool> DeleteClientAsync(int id);
}
