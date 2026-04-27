namespace LogisticsAPI.DTOs;

public class ShipmentResponseDto
{
    public int Id { get; set; }
    public required string Origin { get; set; }
    public required string Destination { get; set; }
    public required string Carrier { get; set; }
    public required string TrackingNumber { get; set; }
    public DateTime EstimatedDeliveryDateUtc { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? DeliveredAtUtc { get; set; }
}
