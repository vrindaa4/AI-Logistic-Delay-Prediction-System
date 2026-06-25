using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories;

public interface IShipmentTrackingRepository
{
    ShipmentTrackingEvent Add(ShipmentTrackingEvent trackingEvent);
    List<ShipmentTrackingEvent> GetByShipmentId(int shipmentId);
}
