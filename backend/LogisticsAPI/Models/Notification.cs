public class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? ShipmentId { get; set; }
    public string? ShipmentNumber { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string Type { get; set; } = "DelayAlert";

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
