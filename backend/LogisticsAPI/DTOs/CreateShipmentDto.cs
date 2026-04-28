using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.DTOs;

public class CreateShipmentDto
{
    [Required]
    public string Origin { get; set; } = string.Empty;

    [Required]
    public string Destination { get; set; } = string.Empty;


    public string? Carrier { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Status { get; set; }

    public DateTime? EstimatedDeliveryDateUtc { get; set; }
}