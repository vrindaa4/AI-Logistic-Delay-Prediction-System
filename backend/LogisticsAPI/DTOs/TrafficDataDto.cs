namespace LogisticsAPI.DTOs;

public class TrafficDataDto
{
    public int DurationInTraffic { get; set; } 
    public int Distance { get; set; } 
    public string TrafficCondition { get; set; } = "UNKNOWN"; // LIGHT, MODERATE, HEAVY, SEVERE
    public int EstimatedDelayMinutes { get; set; }
    public DateTime LastUpdated { get; set; }
}
