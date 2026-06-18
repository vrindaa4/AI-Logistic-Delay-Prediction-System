using LogisticsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAPI.Data;

public class LogisticsDbContext : DbContext
{
    public LogisticsDbContext(DbContextOptions<LogisticsDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<User> Users { get; set; }
    //public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(s => s.User)
            .WithMany(u => u.Shipments)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

            entity.Property(e => e.ShipmentNumber)
                .IsRequired()
                .HasMaxLength(100);


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

            entity.Property(e => e.Weight);

            entity.Property(e => e.EstimatedDeliveryDateUtc);

            entity.Property(e => e.CreatedAtUtc)
                .IsRequired();

            entity.Property(e => e.DeliveredAtUtc);
        });

        
        var seedDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        
modelBuilder.Entity<Shipment>().HasData(
    new Shipment
    {
        Id = 1,
        ShipmentNumber = "SHIP-001",
        Origin = "New York",
        Destination = "Los Angeles",
        Carrier = "FedEx",
        Status = "in-transit",
        TrackingNumber = "FDX123456789",
        Weight = 15.5,
        EstimatedDeliveryDateUtc = seedDate.AddDays(5),
        CreatedAtUtc = seedDate.AddDays(-2)
    },
    new Shipment
    {
        Id = 2,
        ShipmentNumber = "SHIP-002",
        Origin = "Chicago",
        Destination = "Miami",
        Carrier = "UPS",
        Status = "delivered",
        TrackingNumber = "UPS987654321",
        Weight = 8.3,
        EstimatedDeliveryDateUtc = seedDate.AddDays(-1),
        CreatedAtUtc = seedDate.AddDays(-7),
        DeliveredAtUtc = seedDate.AddDays(-1)
    },
    new Shipment
    {
        Id = 3,
        ShipmentNumber = "SHIP-003",
        Origin = "Seattle",
        Destination = "Boston",
        Carrier = "DHL",
        Status = "delayed",
        TrackingNumber = "DHL456789012",
        Weight = 12.0,
        EstimatedDeliveryDateUtc = seedDate.AddDays(-2),
        CreatedAtUtc = seedDate.AddDays(-8)
    },
    new Shipment
    {
        Id = 4,
        ShipmentNumber = "SHIP-004",
        Origin = "Chandigarh",
        Destination = "Delhi",
        Carrier = "FedEx",
        Status = "in-transit",
        TrackingNumber = "FDX234567890",
        Weight = 20.1,
        EstimatedDeliveryDateUtc = seedDate.AddDays(3),
        CreatedAtUtc = seedDate.AddDays(-1)
    },
    new Shipment
    {
        Id = 5,
        ShipmentNumber = "SHIP-005",
        Origin = "Pune",
        Destination = "Hyderabad",
        Carrier = "UPS",
        Status = "in-transit",
        TrackingNumber = "UPS345678901",
        Weight = 6.7,
        EstimatedDeliveryDateUtc = seedDate.AddDays(7),
        CreatedAtUtc = seedDate
    }
);
    }
}
