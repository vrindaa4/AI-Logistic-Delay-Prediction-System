namespace LogisticsAPI.DTOs;

public class NotificationDto
{
    public int Id { get; set; }
    public int? ShipmentId { get; set; }
    public string? ShipmentNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
