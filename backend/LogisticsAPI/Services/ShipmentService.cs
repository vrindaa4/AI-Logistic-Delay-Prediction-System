using LogisticsAPI.Models;
using LogisticsAPI.DTOs;
using LogisticsAPI.Repositories;

namespace LogisticsAPI.Services;

public class ShipmentService
{
    private readonly IShipmentRepository _repo;
    private readonly INotificationService _notificationService;

    public ShipmentService(IShipmentRepository repo, INotificationService notificationService)
    {
        _repo = repo;
        _notificationService = notificationService;
    }

    public List<Shipment> GetAll() => _repo.GetAll();
    public Shipment? GetById(int id) => _repo.GetById(id);
    public List<Shipment> GetByUserId(int userId) => _repo.GetByUserId(userId);
    public Shipment? GetByIdForUser(int id, int userId) => _repo.GetByIdForUser(id, userId);

    public async Task<Shipment> CreateAsync(CreateShipmentDto dto, int userId) 
    {
        var shipment = new Shipment
        {
            UserId = userId,
            Origin = dto.Origin,
            Destination = dto.Destination,
            Status =  "pending",  
            Carrier = string.IsNullOrWhiteSpace(dto.Carrier) ? "Default Carrier" : dto.Carrier,
            TrackingNumber = $"TRK{Guid.NewGuid():N}".Substring(0, 6),
            Weight = dto.Weight,
            EstimatedDeliveryDateUtc = dto.EstimatedDeliveryDateUtc ?? DateTime.UtcNow.AddDays(5),
            CreatedAtUtc = DateTime.UtcNow
        };

        var saved = _repo.Add(shipment);
        saved.ShipmentNumber = $"SHIP-{saved.Id:D3}";
        _repo.Update(saved);
        await _notificationService.NotifyShipmentCreatedAsync(saved);
        return saved;
    }

    public async Task UpdateStatusAsync(int id, string status)
    {
        var shipment = _repo.GetById(id);
        if (shipment == null) return;

        var previousStatus = shipment.Status;
        shipment.Status = status;
        _repo.Update(shipment);

         await _notificationService.NotifyStatusChangedAsync(shipment, previousStatus);
    }

    public void Delete(int id) => _repo.Delete(id);
}
