
using LogisticsAPI.Models;
using LogisticsAPI.DTOs;
using LogisticsAPI.Repositories;

namespace LogisticsAPI.Services;

public class ShipmentService
{
    private readonly IShipmentRepository _repo;

    public ShipmentService(IShipmentRepository repo)
    {
        _repo = repo;
    }

    public List<Shipment> GetAll() => _repo.GetAll();

public Shipment? GetById(int id)
{
    return _repo.GetById(id);
}
    public Shipment Create(CreateShipmentDto dto)
    {
        var shipment = new Shipment
        {
            Origin = dto.Origin,
            Destination = dto.Destination,
            Status = "Created",
            Carrier = "Default Carrier",
    TrackingNumber = Guid.NewGuid().ToString(),
    ExpectedDeliveryDate = DateTime.Now.AddDays(5)
        };

        return _repo.Add(shipment);
    }

    public void UpdateStatus(int id, string status)
    {
        var shipment = _repo.GetById(id);
        if (shipment != null)
        {
            shipment.Status = status;
            _repo.Update(shipment);
        }
    }

    public void Delete(int id) => _repo.Delete(id);
}