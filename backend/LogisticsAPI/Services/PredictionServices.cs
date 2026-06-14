using LogisticsAPI.DTOs;

namespace LogisticsAPI.Services;

public class PredictionService
{
    private readonly IAiPredictionService _ai;

    public PredictionService(IAiPredictionService ai)
    {
        _ai = ai;
    }

    public Task<PredictionResponseDto> PredictDelayAsync(PredictionRequestDto request)
        => _ai.PredictAsync(request);
}