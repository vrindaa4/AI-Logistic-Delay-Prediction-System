namespace LogisticsAPI.DTOs;

public class TrackingEventDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public DateTime TimestampUtc { get; set; }
}
