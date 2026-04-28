using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories;
public class ShipmentRepository : IShipmentRepository
{
    private static List<Shipment> shipments = new();

    public List<Shipment> GetAll() => shipments;

    public Shipment GetById(int id) =>
        shipments.FirstOrDefault(s => s.Id == id);

    public Shipment Add(Shipment shipment)
    {
        shipment.Id = shipments.Count + 1;
        shipments.Add(shipment);
        return shipment;
    }

    public void Update(Shipment shipment)
    {
        var existing = GetById(shipment.Id);
        if (existing != null)
        {
            existing.Status = shipment.Status;
        }
    }

    public void Delete(int id)
    {
        var shipment = GetById(id);
        if (shipment != null)
            shipments.Remove(shipment);
    }
}