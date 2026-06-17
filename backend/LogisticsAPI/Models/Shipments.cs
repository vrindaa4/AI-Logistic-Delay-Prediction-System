using System.Text.Json.Serialization;

namespace LogisticsAPI.Models;

public class Shipment
{
    public int Id { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public string ShipmentNumber { get; set; } = string.Empty;

    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public string Carrier { get; set; } = string.Empty;

    public string TrackingNumber { get; set; } = string.Empty;

    public double Weight { get; set; }

    public DateTime? EstimatedDeliveryDateUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? DeliveredAtUtc { get; set; }
}