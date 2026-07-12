using Microsoft.AspNetCore.Mvc;
using OrdersApi.Dtos;
using OrdersApi.Services;

namespace OrdersApi.Controllers;

/// <summary>Отчеты на основе хранимых процедур БД (задание 2).</summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reports;

    public ReportsController(IReportService reports)
    {
        _reports = reports;
    }

    /// <summary>
    /// Сумма заказов со статусом "Выполнен" по каждому клиенту,
    /// произведенных в день рождения клиента.
    /// </summary>
    [HttpGet("birthday-orders-total")]
    public async Task<ActionResult<List<BirthdayOrdersTotalDto>>> GetBirthdayOrdersTotal()
    {
        return Ok(await _reports.GetBirthdayOrdersTotalAsync());
    }

    /// <summary>
    /// Список часов от 00 до 23 в порядке убывания со средним чеком
    /// за каждый час по заказам со статусом "Выполнен".
    /// </summary>
    [HttpGet("hourly-average-check")]
    public async Task<ActionResult<List<HourlyAverageCheckDto>>> GetHourlyAverageCheck()
    {
        return Ok(await _reports.GetHourlyAverageCheckAsync());
    }
}
