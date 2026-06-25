using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.DTOs;

public class CreateShipmentDto
{
    [Required]
    public string Origin { get; set; } = string.Empty;

    [Required]
    public string Destination { get; set; } = string.Empty;


    public string? Carrier { get; set; }
    [RegularExpression(@"^(?!string$).*", ErrorMessage = "Please enter a valid tracking number")]
    public string? TrackingNumber { get; set; }

     public double Weight { get; set; }    
    
    public DateTime? EstimatedDeliveryDateUtc { get; set; }
   
}