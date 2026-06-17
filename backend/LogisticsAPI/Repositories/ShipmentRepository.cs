using LogisticsAPI.Data;
using LogisticsAPI.Models;
using LogisticsAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAPI.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly LogisticsDbContext _context;

    public ShipmentRepository(LogisticsDbContext context)
    {
        _context = context;
    }

    public List<Shipment> GetAll()
    {
        return _context.Shipments
            .AsNoTracking()
            .Include(s => s.User)
            .OrderByDescending(s => s.CreatedAtUtc)
            .ToList();
    }

    public List<Shipment> GetByUserId(int userId)
    {
        return _context.Shipments
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .ToList();
    }

    public List<Shipment> Search(ShipmentSearchDto criteria)
    {
        var query = _context.Shipments
            .AsNoTracking()
            .Include(s => s.User)
            .AsQueryable();

        if (criteria.UserId.HasValue)
            query = query.Where(s => s.UserId == criteria.UserId.Value);

        if (!string.IsNullOrWhiteSpace(criteria.Status))
            query = query.Where(s => s.Status == criteria.Status);

        if (!string.IsNullOrWhiteSpace(criteria.Carrier))
            query = query.Where(s => s.Carrier.Contains(criteria.Carrier));

        if (!string.IsNullOrWhiteSpace(criteria.Query))
        {
            var term = criteria.Query.Trim();
            query = query.Where(s =>
                s.ShipmentNumber.Contains(term) ||
                s.Origin.Contains(term) ||
                s.Destination.Contains(term) ||
                s.TrackingNumber.Contains(term) ||
                s.Carrier.Contains(term));
        }

        return query.OrderByDescending(s => s.CreatedAtUtc).ToList();
    }

    public Shipment? GetById(int id)
    {
        return _context.Shipments
            .AsNoTracking()
            .Include(s => s.User)
            .FirstOrDefault(s => s.Id == id);
    }

    public Shipment? GetByIdForUser(int id, int userId)
    {
        return _context.Shipments
            .AsNoTracking()
            .FirstOrDefault(s => s.Id == id && s.UserId == userId);
    }

    public Shipment Add(Shipment shipment)
    {
        shipment.CreatedAtUtc = DateTime.UtcNow;
        _context.Shipments.Add(shipment);
        _context.SaveChanges();
        return shipment;
    }

    public void Update(Shipment shipment)
    {
        var existing = _context.Shipments.FirstOrDefault(s => s.Id == shipment.Id);
        if (existing != null)
        {
            existing.Status = shipment.Status;
            _context.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var shipment = _context.Shipments.FirstOrDefault(s => s.Id == id);
        if (shipment != null)
        {
            _context.Shipments.Remove(shipment);
            _context.SaveChanges();
        }
    }

    public int Count() => _context.Shipments.Count();

    public int CountByUserId(int userId) =>
        _context.Shipments.Count(s => s.UserId == userId);

    public Dictionary<string, int> CountByCarrier() =>
        _context.Shipments
            .GroupBy(s => s.Carrier)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionary(x => x.Key, x => x.Count);

    public Dictionary<string, int> CountByStatus() =>
        _context.Shipments
            .GroupBy(s => s.Status)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionary(x => x.Key, x => x.Count);
}
