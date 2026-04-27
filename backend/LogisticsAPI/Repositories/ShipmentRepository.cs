using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private static List<Shipment> shipments = new();

        public List<Shipment> GetAll() => shipments;

        public Shipment Add(Shipment shipment)
        {
            shipment.Id = shipments.Count + 1;
            shipments.Add(shipment);
            return shipment;
        }
    }
}