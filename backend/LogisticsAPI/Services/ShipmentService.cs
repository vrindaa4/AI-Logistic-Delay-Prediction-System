using LogisticsAPI.Models;
using LogisticsAPI.Repositories;

namespace LogisticsAPI.Services
{
    public class ShipmentService
    {
        private readonly IShipmentRepository _repo;

        public ShipmentService(IShipmentRepository repo)
        {
            _repo = repo;
        }

        public List<Shipment> GetShipments()
        {
            return _repo.GetAll();
        }

        public Shipment CreateShipment(Shipment shipment)
        {
            shipment.Status = "Pending";
            return _repo.Add(shipment);
        }
    }
}