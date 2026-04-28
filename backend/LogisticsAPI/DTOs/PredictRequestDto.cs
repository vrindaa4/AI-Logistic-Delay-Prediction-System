namespace LogisticsAPI.DTOs;

public class PredictionRequestDto
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double WeatherScore { get; set; }
}