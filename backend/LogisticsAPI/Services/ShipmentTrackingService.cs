using LogisticsAPI.DTOs;
using LogisticsAPI.Hubs;
using LogisticsAPI.Models;
using LogisticsAPI.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsAPI.Services;

public class ShipmentTrackingService : IShipmentTrackingService
{
    private readonly IShipmentTrackingRepository _repo;
    private readonly IHubContext<TrackingHub> _hub;
    private readonly ILogger<ShipmentTrackingService> _logger;

    public ShipmentTrackingService(
        IShipmentTrackingRepository repo,
        IHubContext<TrackingHub> hub,
        ILogger<ShipmentTrackingService> logger)
    {
        _repo = repo;
        _hub = hub;
        _logger = logger;
    }

    public async Task RecordEventAsync(Shipment shipment, string status, string? location = null, string? notes = null)
    {
        var trackingEvent = new ShipmentTrackingEvent
        {
            ShipmentId = shipment.Id,
            Status = status,
            Location = location,
            Notes = notes
        };

        var saved = _repo.Add(trackingEvent);
        var dto = ToDto(saved);

        try
        {
            await _hub.Clients
                .Group(TrackingHub.GroupName(shipment.Id))
                .SendAsync("TrackingUpdated", dto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Tracking event saved but real-time push failed for shipment {ShipmentId}", shipment.Id);
        }
    }

    public List<TrackingEventDto> GetHistory(int shipmentId)
        => _repo.GetByShipmentId(shipmentId).Select(ToDto).ToList();

    private static TrackingEventDto ToDto(ShipmentTrackingEvent e) => new()
    {
        Id = e.Id,
        Status = e.Status,
        Location = e.Location,
        Notes = e.Notes,
        TimestampUtc = e.TimestampUtc
    };
}
