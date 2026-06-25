using LogisticsAPI.DTOs;
using LogisticsAPI.Models;

namespace LogisticsAPI.Services;

public interface IShipmentTrackingService
{
 
    Task RecordEventAsync(Shipment shipment, string status, string? location = null, string? notes = null);

    List<TrackingEventDto> GetHistory(int shipmentId);
}
