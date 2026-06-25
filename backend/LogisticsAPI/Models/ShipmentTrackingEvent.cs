namespace LogisticsAPI.Models;

public class ShipmentTrackingEvent
{
    public int Id { get; set; }

    public int ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Location { get; set; }
    public string? Notes { get; set; }

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}
