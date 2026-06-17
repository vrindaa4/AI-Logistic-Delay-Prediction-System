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

    // Admin: all shipments
    public List<Shipment> GetAll() => _repo.GetAll();

    // Admin: any shipment by id
    public Shipment? GetById(int id) => _repo.GetById(id);


    // User: only their own shipments
    public List<Shipment> GetByUserId(int userId) => _repo.GetByUserId(userId);
    public Shipment? GetByIdForUser(int id, int userId) => _repo.GetByIdForUser(id, userId);


    public Shipment Create(CreateShipmentDto dto, int userId)
    {
        var shipment = new Shipment
        {
            UserId = userId,
            Origin = dto.Origin,
            Destination = dto.Destination,
            Status = "in-transit",
            Carrier = string.IsNullOrWhiteSpace(dto.Carrier) ? "Default Carrier" : dto.Carrier,
            TrackingNumber = string.IsNullOrWhiteSpace(dto.TrackingNumber)
                ? Guid.NewGuid().ToString()
                : dto.TrackingNumber,
            EstimatedDeliveryDateUtc = dto.EstimatedDeliveryDateUtc ?? DateTime.UtcNow.AddDays(5),
            CreatedAtUtc = DateTime.UtcNow
        };

    // Save first to get the DB-assigned Id
        var saved = _repo.Add(shipment);

        // Auto-generate shipment number from Id
        saved.ShipmentNumber = $"SHIP-{saved.Id:D3}";
        _repo.Update(saved);

        return saved;    }

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
