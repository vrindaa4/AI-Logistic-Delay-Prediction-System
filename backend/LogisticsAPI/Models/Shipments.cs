namespace LogisticsAPI.Models;

public class Shipment
{
    public int Id { get; set; }

    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public string Carrier { get; set; } = string.Empty;

    public string TrackingNumber { get; set; } = string.Empty;

    public DateTime? EstimatedDeliveryDateUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? DeliveredAtUtc { get; set; }
}