using LogisticsAPI.Data;
using LogisticsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAPI.Repositories;

public class ShipmentTrackingRepository : IShipmentTrackingRepository
{
    private readonly LogisticsDbContext _context;

    public ShipmentTrackingRepository(LogisticsDbContext context)
    {
        _context = context;
    }

    public ShipmentTrackingEvent Add(ShipmentTrackingEvent trackingEvent)
    {
        trackingEvent.TimestampUtc = DateTime.UtcNow;
        _context.ShipmentTrackingEvents.Add(trackingEvent);
        _context.SaveChanges();
        return trackingEvent;
    }


    public List<ShipmentTrackingEvent> GetByShipmentId(int shipmentId)
        => _context.ShipmentTrackingEvents
            .Where(e => e.ShipmentId == shipmentId)
            .OrderByDescending(e => e.TimestampUtc)
            .ToList();
}
