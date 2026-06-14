namespace LogisticsAPI.DTOs;

public class TrafficDataDto
{
    public int DurationInTraffic { get; set; }
    public int Distance { get; set; }
    public string TrafficCondition { get; set; } = "UNKNOWN";
    public int EstimatedDelayMinutes { get; set; }
    public string? WeatherCondition { get; set; }
    public double WeatherRiskScore { get; set; }
    public string DataSource { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}
