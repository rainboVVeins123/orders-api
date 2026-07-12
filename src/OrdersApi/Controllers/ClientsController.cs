using Microsoft.AspNetCore.Mvc;
using OrdersApi.Dtos;
using OrdersApi.Models;
using OrdersApi.Services;

namespace OrdersApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clients;

    public ClientsController(IClientService clients)
    {
        _clients = clients;
    }

    /// <summary>Получение клиентов с фильтрацией по всем полям и постраничным просмотром.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<Client>>> GetClients([FromQuery] ClientFilterDto filter)
    {
        return Ok(await _clients.GetClientsAsync(filter));
    }

    /// <summary>Получение клиента по идентификатору.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _clients.GetClientByIdAsync(id);
        return client is null ? NotFound() : Ok(client);
    }

    /// <summary>Создание клиента.</summary>
    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(ClientCreateDto dto)
    {
        var client = await _clients.CreateClientAsync(dto);
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
    }

    /// <summary>Редактирование клиента.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Client>> UpdateClient(int id, ClientUpdateDto dto)
    {
        var client = await _clients.UpdateClientAsync(id, dto);
        return client is null ? NotFound() : Ok(client);
    }

    /// <summary>Удаление клиента.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var deleted = await _clients.DeleteClientAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
