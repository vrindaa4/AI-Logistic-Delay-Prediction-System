using LogisticsAPI.DTOs;
using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories;

public interface IShipmentRepository
{
    List<Shipment> GetAll();
    Shipment Add(Shipment shipment);
}
