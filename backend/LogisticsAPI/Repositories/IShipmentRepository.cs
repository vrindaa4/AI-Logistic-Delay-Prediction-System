using LogisticsAPI.DTOs;
using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories;

public interface IShipmentRepository
{
    List<Shipment> GetAll();

    Shipment? GetById(int id);
    Shipment Add(Shipment shipment);
    void Update(Shipment shipment);
    void Delete(int id);
}
