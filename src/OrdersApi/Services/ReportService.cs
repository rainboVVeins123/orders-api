using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Dtos;

namespace OrdersApi.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _db;

    public ReportService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<BirthdayOrdersTotalDto>> GetBirthdayOrdersTotalAsync()
    {
        return await _db.Database
            .SqlQuery<BirthdayOrdersTotalDto>($"""
                SELECT client_id    AS "ClientId",
                       first_name   AS "FirstName",
                       last_name    AS "LastName",
                       total_amount AS "TotalAmount"
                FROM get_birthday_orders_total()
                """)
            .ToListAsync();
    }

    public async Task<List<HourlyAverageCheckDto>> GetHourlyAverageCheckAsync()
    {
        return await _db.Database
            .SqlQuery<HourlyAverageCheckDto>($"""
                SELECT hour          AS "Hour",
                       average_check AS "AverageCheck"
                FROM get_hourly_average_check()
                """)
            .ToListAsync();
    }
}
