using BCrypt.Net;
using LogisticsAPI.Models;

namespace LogisticsAPI.Data;

public static class DbSeeder
{
    private const string DemoEmail = "demo@logistics.com";
    private const string DemoPassword = "Demo@123";

    public static void Seed(LogisticsDbContext context)
    {
        // 1. Create demo user if not exists
        var demoUser = context.Users.FirstOrDefault(u => u.Email == DemoEmail);

        if (demoUser == null)
        {
            demoUser = new User
            {
                Email        = DemoEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(DemoPassword),
                Name         = "Demo User"
            };

            context.Users.Add(demoUser);
            context.SaveChanges();
        }

        // 2. Seed demo shipments if the demo user has none
        bool demoHasShipments = context.Shipments.Any(s => s.UserId == demoUser.Id);

        if (!demoHasShipments)
        {
            var demoShipments = new List<Shipment>
{
    new Shipment
    {
        ShipmentNumber           = "SHIP-001",
        Origin                   = "New York",
        Destination              = "Los Angeles",
        Carrier                  = "FedEx",
        Status                   = "in-transit",
        EstimatedDeliveryDateUtc = new DateTime(2026, 6, 6),
        UserId                   = demoUser.Id,
        CreatedAtUtc             = DateTime.UtcNow
    },
    new Shipment
    {
        ShipmentNumber           = "SHIP-002",
        Origin                   = "Chicago",
        Destination              = "Miami",
        Carrier                  = "UPS",
        Status                   = "delivered",
        EstimatedDeliveryDateUtc = new DateTime(2026, 5, 31),
        UserId                   = demoUser.Id,
        CreatedAtUtc             = DateTime.UtcNow
    },
    new Shipment
    {
        ShipmentNumber           = "SHIP-003",
        Origin                   = "Seattle",
        Destination              = "Boston",
        Carrier                  = "DHL",
        Status                   = "delayed",
        EstimatedDeliveryDateUtc = new DateTime(2026, 5, 30),
        UserId                   = demoUser.Id,
        CreatedAtUtc             = DateTime.UtcNow
    },
    new Shipment
    {
        ShipmentNumber           = "SHIP-004",
        Origin                   = "Chandigarh",
        Destination              = "Delhi",
        Carrier                  = "FedEx",
        Status                   = "in-transit",
        EstimatedDeliveryDateUtc = new DateTime(2026, 6, 4),
        UserId                   = demoUser.Id,
        CreatedAtUtc             = DateTime.UtcNow
    },
    new Shipment
    {
        ShipmentNumber           = "SHIP-005",
        Origin                   = "Pune",
        Destination              = "Hyderabad",
        Carrier                  = "UPS",
        Status                   = "in-transit",
        EstimatedDeliveryDateUtc = new DateTime(2026, 6, 8),
        UserId                   = demoUser.Id,
        CreatedAtUtc             = DateTime.UtcNow
    }
};

            context.Shipments.AddRange(demoShipments);
            context.SaveChanges();
        }

        // 3. Reassign any orphaned shipments to demo user
        var orphanedShipments = context.Shipments
            .Where(s => s.UserId == null)
            .ToList();

        if (orphanedShipments.Count > 0)
        {
            foreach (var shipment in orphanedShipments)
            {
                shipment.UserId = demoUser.Id;
            }

            context.SaveChanges();
        }
    }
}