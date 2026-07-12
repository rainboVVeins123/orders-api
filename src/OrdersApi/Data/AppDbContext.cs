using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;

namespace OrdersApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.FirstName).HasColumnName("first_name").HasMaxLength(100);
            entity.Property(c => c.LastName).HasColumnName("last_name").HasMaxLength(100);
            entity.Property(c => c.BirthDate).HasColumnName("birth_date");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).HasColumnName("id");
            entity.Property(o => o.Amount).HasColumnName("amount").HasColumnType("numeric(18,2)");
            entity.Property(o => o.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone");
            entity.Property(o => o.Status).HasColumnName("status").HasConversion<short>();
            entity.Property(o => o.ClientId).HasColumnName("client_id");

            entity.HasOne(o => o.Client)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.ClientId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
