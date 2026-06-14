using System.Text.Json.Serialization;

namespace LogisticsAPI.DTOs;

public class PredictionResponseDto
{
   
    public double DelayProbability { get; set; }

    public int EstimatedDelayDays { get; set; }
    public string RiskLevel { get; set; } = "low";
    public string Recommendation { get; set; } = string.Empty;
    public string AiExplanation { get; set; } = string.Empty;
    public string PredictionSource { get; set; } = "rule-based";
    public TrafficDataDto? TrafficData { get; set; }
    public Dictionary<string, string> Factors { get; set; } = new();
}
