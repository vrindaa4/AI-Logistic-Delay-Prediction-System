namespace LogisticsAPI.DTOs;

public class TrafficDataDto
{
    public int DurationInTraffic { get; set; } // seconds
    public int Distance { get; set; } // meters
    public string TrafficCondition { get; set; } = "UNKNOWN"; // LIGHT, MODERATE, HEAVY, SEVERE
    public int EstimatedDelayMinutes { get; set; }
    public DateTime LastUpdated { get; set; }
}
