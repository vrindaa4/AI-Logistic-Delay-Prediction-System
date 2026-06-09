using System.ComponentModel.DataAnnotations;
namespace LogisticsAPI.DTOs;

public class ShipmentResponseDto
{
    public int Id { get; set; }
    [Required]
    public string? Origin { get; set; }
    [Required]
    public string? Destination { get; set; }
    [Required]
    public string? Carrier { get; set; }
    [Required]
    public string? TrackingNumber { get; set; }
    public DateTime EstimatedDeliveryDateUtc { get; set; }
    [Required]
    public string? Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? DeliveredAtUtc { get; set; }
}