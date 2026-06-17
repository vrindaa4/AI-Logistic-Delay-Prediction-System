namespace LogisticsAPI.DTOs;

public class ShipmentSearchDto
{
    public string? Query { get; set; }
    public string? Status { get; set; }
    public string? Carrier { get; set; }
    public int? UserId { get; set; }
}
