using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogisticsAPI.DTOs;

namespace LogisticsAPI.Services;

public interface IAiPredictionService
{
    Task<PredictionResponseDto> PredictAsync(PredictionRequestDto request);
}

public class AiPredictionService : IAiPredictionService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<AiPredictionService> _logger;

    public AiPredictionService(HttpClient httpClient, IConfiguration config, ILogger<AiPredictionService> logger)
    {
        _httpClient = httpClient;
        _logger     = logger;
        _apiKey     = config["Ai:GroqApiKey"] ?? "";
    }

    public async Task<PredictionResponseDto> PredictAsync(PredictionRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
            return Fallback(request, "No Groq API key configured.");

        var prompt = $$"""
            You are a logistics delay prediction expert.
            Analyze this shipment and return ONLY raw JSON with no markdown, no explanation, no code blocks.

            Shipment:
            - Origin: {{request.Origin}}
            - Destination: {{request.Destination}}
            - Carrier: {{request.Carrier}}
            - Weight: {{request.Weight}} kg

            Consider route distance, road conditions, carrier reliability
            (FedEx/UPS/DHL = reliable, others = less reliable), and weight.

            You MUST respond with ONLY this JSON structure, nothing else before or after:
            {"delayProbability":0.2,"estimatedDelayDays":0,"riskLevel":"low","recommendation":"your recommendation here","explanation":"your two sentence explanation here"}
            """;

        try
        {
            var payload = new
            {
                model       = "llama-3.3-70b-versatile",
                temperature = 0.2,
                messages    = new[]
                {
                    new { role = "system", content = "You are a logistics expert. Always respond with raw JSON only. Never use markdown or code blocks." },
                    new { role = "user",   content = prompt }
                }
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsJsonAsync(
                "https://api.groq.com/openai/v1/chat/completions", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Groq error: {Status} {Error}", response.StatusCode, error);
                return Fallback(request, $"Groq API error: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<GroqResponse>();
            var text   = result?.Choices?[0]?.Message?.Content;

            _logger.LogInformation("Groq raw response: {Text}", text);

            if (string.IsNullOrWhiteSpace(text))
                return Fallback(request, "Empty response from Groq.");

            // Strip markdown fences if present
            text = text.Trim();
            if (text.Contains("```"))
            {
                var fenceStart = text.IndexOf("```");
                var fenceEnd   = text.LastIndexOf("```");
                if (fenceEnd > fenceStart)
                {
                    text = text[(fenceStart + 3)..fenceEnd];
                    var newline = text.IndexOf('\n');
                    if (newline >= 0 && newline < 10)
                        text = text[(newline + 1)..];
                }
            }

            // Extract JSON object
            var start = text.IndexOf('{');
            var end   = text.LastIndexOf('}');

            if (start < 0 || end <= start)
            {
                _logger.LogWarning("No JSON found in Groq response: {Text}", text);
                return Fallback(request, "Invalid JSON from Groq.");
            }

            text = text[start..(end + 1)];
            _logger.LogInformation("Extracted JSON: {Text}", text);

            var parsed = JsonSerializer.Deserialize<AiResult>(text, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (parsed == null)
                return Fallback(request, "Could not parse Groq response.");

            _logger.LogInformation("Groq prediction succeeded for {Origin} → {Destination}",
                request.Origin, request.Destination);

            return new PredictionResponseDto
            {
                DelayProbability   = Math.Clamp(Math.Round(parsed.DelayProbability, 2), 0, 0.99),
                EstimatedDelayDays = Math.Max(0, parsed.EstimatedDelayDays),
                RiskLevel          = parsed.RiskLevel?.ToLower() is "low" or "medium" or "high"
                                        ? parsed.RiskLevel.ToLower() : "medium",
                Recommendation     = parsed.Recommendation ?? "Monitor shipment closely.",
                AiExplanation      = parsed.Explanation    ?? "Based on AI analysis.",
                PredictionSource   = "ai (Groq)",
                Factors = new Dictionary<string, string>
                {
                    ["carrier"] = string.IsNullOrWhiteSpace(request.Carrier) ? "Unknown" : request.Carrier,
                    ["weight"]  = request.Weight switch { > 1000 => "Heavy (>1000kg)", > 500 => "Moderate", _ => "Normal" },
                    ["route"]   = $"{request.Origin} → {request.Destination}",
                    ["source"]  = "Groq / llama-3.3-70b-versatile"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Groq request failed");
            return Fallback(request, ex.Message);
        }
    }

    private static PredictionResponseDto Fallback(PredictionRequestDto request, string reason)
    {
        var reliability = request.Carrier?.Trim().ToLowerInvariant() switch
        {
            "fedex" or "ups" or "dhl" => 0.9,
            _ => 0.7
        };
        var probability = Math.Round(Math.Clamp(
            0.2 + (request.Weight switch { > 1000 => 0.15, > 500 => 0.08, _ => 0 })
                + (1 - reliability) * 0.2, 0, 0.99), 2);

        return new PredictionResponseDto
        {
            DelayProbability   = probability,
            EstimatedDelayDays = probability > 0.5 ? 1 : 0,
            RiskLevel          = probability >= 0.7 ? "high" : probability >= 0.4 ? "medium" : "low",
            Recommendation     = "Add Groq API key at console.groq.com for AI predictions.",
            AiExplanation      = $"Rule-based fallback. Reason: {reason}",
            PredictionSource   = "fallback",
            Factors = new Dictionary<string, string>
            {
                ["carrier"] = request.Carrier ?? "Unknown",
                ["weight"]  = request.Weight switch { > 1000 => "Heavy", > 500 => "Moderate", _ => "Normal" },
                ["note"]    = reason
            }
        };
    }

    private class GroqResponse
    {
        [JsonPropertyName("choices")] public List<GroqChoice>? Choices { get; set; }
    }
    private class GroqChoice
    {
        [JsonPropertyName("message")] public GroqMessage? Message { get; set; }
    }
    private class GroqMessage
    {
        [JsonPropertyName("content")] public string? Content { get; set; }
    }
    private class AiResult
    {
        [JsonPropertyName("delayProbability")]   public double  DelayProbability   { get; set; }
        [JsonPropertyName("estimatedDelayDays")] public int    EstimatedDelayDays { get; set; }
        [JsonPropertyName("riskLevel")]          public string? RiskLevel          { get; set; }
        [JsonPropertyName("recommendation")]     public string? Recommendation     { get; set; }
        [JsonPropertyName("explanation")]        public string? Explanation        { get; set; }
    }
}