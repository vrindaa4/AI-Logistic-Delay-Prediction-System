using Microsoft.AspNetCore.Mvc;
using LogisticsAPI.Models;
using LogisticsAPI.Services;

namespace LogisticsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly ShipmentService _service;

        public ShipmentController(ShipmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetShipments());
        }

        [HttpPost]
        public IActionResult Create(Shipment shipment)
        {
            return Ok(_service.CreateShipment(shipment));
        }
    }
}