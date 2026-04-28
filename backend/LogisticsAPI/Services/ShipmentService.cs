public class ShipmentService
{
    private readonly IShipmentRepository _repo;

    public ShipmentService(IShipmentRepository repo)
    {
        _repo = repo;
    }

    public List<Shipment> GetAll() => _repo.GetAll();

    public Shipment GetById(int id) => _repo.GetById(id);

    public Shipment Create(CreateShipmentDto dto)
    {
        var shipment = new Shipment
        {
            Origin = dto.Origin,
            Destination = dto.Destination,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            Status = "Pending"
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