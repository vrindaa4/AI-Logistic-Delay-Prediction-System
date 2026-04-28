using LogisticsAPI.Models;
using LogisticsAPI.DTOs;
using LogisticsAPI.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private static List<Shipment> shipments = new();
    private static int _currentId = 1;

    public List<Shipment> GetAll() => new List<Shipment>(shipments);

    public Shipment? GetById(int id)
    {
        return shipments.FirstOrDefault(s => s.Id == id);
    }

    public Shipment Add(Shipment shipment)
    {
        shipment.Id = _currentId++;
        shipments.Add(shipment);
        return shipment;
    }

    public void Update(Shipment shipment)
    {
        var existing = shipments.FirstOrDefault(s => s.Id == shipment.Id);

        if (existing != null)
        {
            existing.Status = shipment.Status;
        }
    }

    public void Delete(int id)
    {
        var shipment = shipments.FirstOrDefault(s => s.Id == id);

        if (shipment != null)
            shipments.Remove(shipment);
    }
}