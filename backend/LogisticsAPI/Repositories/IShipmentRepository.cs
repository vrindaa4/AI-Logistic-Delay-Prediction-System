using LogisticsAPI.DTOs;
using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories;

public interface IShipmentRepository
{
    Task<IEnumerable<Shipment>> GetAllAsync();
    Task<Shipment?> GetByIdAsync(int id);
    Task<Shipment> CreateAsync(Shipment shipment);
    Task<Shipment?> UpdateAsync(int id, Shipment shipment);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Shipment>> SearchAsync(string searchTerm);
    Task<IEnumerable<Shipment>> FilterByStatusAsync(string status);
    Task<IEnumerable<Shipment>> FilterByCarrierAsync(string carrier);
    Task<IEnumerable<Shipment>> GetDelayedShipmentsAsync();
}
