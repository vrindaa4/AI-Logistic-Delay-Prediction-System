using LogisticsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAPI.Data;

public class LogisticsDbContext : DbContext
{
    public LogisticsDbContext(DbContextOptions<LogisticsDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Origin)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Destination)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Carrier)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.TrackingNumber)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.EstimatedDeliveryDateUtc);

            entity.Property(e => e.CreatedAtUtc)
                .IsRequired();

            entity.Property(e => e.DeliveredAtUtc);
        });
    }
}
