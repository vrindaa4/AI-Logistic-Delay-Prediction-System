namespace LogisticsAPI.DTOs
{
    public class CreateShipmentDto
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
    }
}