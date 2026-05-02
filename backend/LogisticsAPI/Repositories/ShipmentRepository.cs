using LogisticsAPI.Models;
using LogisticsAPI.DTOs;
using LogisticsAPI.Data;
using LogisticsAPI.Repositories;
using Microsoft.EntityFrameworkCore;

public class ShipmentRepository : IShipmentRepository
{
    private readonly LogisticsDbContext _context;

    public ShipmentRepository(LogisticsDbContext context)
    {
        _context = context;
    }

    public List<Shipment> GetAll()
    {
        return _context.Shipments.ToList();
    }

    public Shipment? GetById(int id)
    {
        return _context.Shipments.FirstOrDefault(s => s.Id == id);
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
}