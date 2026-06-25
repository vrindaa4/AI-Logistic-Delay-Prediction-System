using LogisticsAPI.DTOs;
using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories;

public interface IShipmentRepository
{
    List<Shipment> GetAll();
    List<Shipment> GetByUserId(int userId);
    Shipment? GetById(int id);
    Shipment? GetByIdForUser(int id, int userId);
    bool ExistsByTrackingNumber(string trackingNumber);
    Shipment Add(Shipment shipment);
    void Update(Shipment shipment);
    void Delete(int id);
}
