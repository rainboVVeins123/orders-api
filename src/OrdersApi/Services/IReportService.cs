using OrdersApi.Dtos;

namespace OrdersApi.Services;

/// <summary>Отчеты на основе хранимых процедур БД.</summary>
public interface IReportService
{
    Task<List<BirthdayOrdersTotalDto>> GetBirthdayOrdersTotalAsync();

    Task<List<HourlyAverageCheckDto>> GetHourlyAverageCheckAsync();
}
