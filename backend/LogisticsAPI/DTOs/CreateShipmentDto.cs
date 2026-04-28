
namespace LogisticsAPI.DTOs;

public class CreateShipmentDto
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
}