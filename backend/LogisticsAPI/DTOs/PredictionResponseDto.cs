namespace LogisticsAPI.DTOs;

public class PredictionResponseDto
{
    public double DelayProbability { get; set; } // 0-1
    public int EstimatedDelayDays { get; set; }
    public string RiskLevel { get; set; } = "LOW"; // LOW, MEDIUM, HIGH
    public string Recommendation { get; set; } = string.Empty;
    public TrafficDataDto? TrafficData { get; set; }
    public Dictionary<string, string> Factors { get; set; } = new();
}
