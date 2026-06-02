using System.Net.Http.Json;
using LogisticsAPI.DTOs;

namespace LogisticsAPI.Services;

public interface ITrafficService
{
    Task<TrafficDataDto?> GetTrafficDataAsync(string origin, string destination);
}

public class TrafficService : ITrafficService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TrafficService> _logger;

    public TrafficService(HttpClient httpClient, IConfiguration configuration, ILogger<TrafficService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

   
    public async Task<TrafficDataDto?> GetTrafficDataAsync(string origin, string destination)
    {
        try
        {
            var apiKey = _configuration["TrafficApi:GoogleMapsApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("Google Maps API key not configured");
                return null;
            }

            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json" +
                     $"?origins={Uri.EscapeDataString(origin)}" +
                     $"&destinations={Uri.EscapeDataString(destination)}" +
                     $"&key={apiKey}" +
                     $"&departure_time=now";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<GoogleDistanceMatrixResponse>();

            if (result?.rows?.FirstOrDefault()?.elements?.FirstOrDefault() != null)
            {
                var element = result.rows[0].elements[0];
                
                return new TrafficDataDto
                {
                    DurationInTraffic = element.duration_in_traffic?.value ?? element.duration.value,
                    Distance = element.distance.value,
                    TrafficCondition = DetermineTrafficCondition(element),
                    EstimatedDelayMinutes = CalculateDelayMinutes(element),
                    LastUpdated = DateTime.UtcNow
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching traffic data: {ex.Message}");
            return null;
        }
    }

    private string DetermineTrafficCondition(GoogleDistanceElement element)
    {
        if (element.duration_in_traffic == null)
            return "UNKNOWN";

        var normalDuration = element.duration.value;
        var trafficDuration = element.duration_in_traffic.value;
        var delayRatio = (double)trafficDuration / normalDuration;

        return delayRatio switch
        {
            < 1.1 => "LIGHT",
            < 1.3 => "MODERATE",
            < 1.5 => "HEAVY",
            _ => "SEVERE"
        };
    }

    private int CalculateDelayMinutes(GoogleDistanceElement element)
    {
        if (element.duration_in_traffic == null)
            return 0;

        var normalSeconds = element.duration.value;
        var trafficSeconds = element.duration_in_traffic.value;
        var delaySeconds = trafficSeconds - normalSeconds;

        return Math.Max(0, delaySeconds / 60);
    }
}

// Google Maps API Response Models
public class GoogleDistanceMatrixResponse
{
    public string status { get; set; }
    public string[] origin_addresses { get; set; }
    public string[] destination_addresses { get; set; }
    public GoogleRow[] rows { get; set; }
}

public class GoogleRow
{
    public GoogleDistanceElement[] elements { get; set; }
}

public class GoogleDistanceElement
{
    public GoogleStatus status { get; set; }
    public GoogleDuration duration { get; set; }
    public GoogleDuration duration_in_traffic { get; set; }
    public GoogleDistance distance { get; set; }
}

public class GoogleStatus
{
    public string status { get; set; }
}

public class GoogleDuration
{
    public string text { get; set; }
    public int value { get; set; } // seconds
}

public class GoogleDistance
{
    public string text { get; set; }
    public int value { get; set; } // meters
}
