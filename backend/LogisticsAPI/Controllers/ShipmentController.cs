using Microsoft.AspNetCore.Mvc;
using LogisticsAPI.Models;
using LogisticsAPI.Services;
using LogisticsAPI.DTOs;
namespace LogisticsAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ShipmentController : ControllerBase
{
    private readonly ShipmentService _service;
    private readonly PredictionService _predictionService;

    public ShipmentController(ShipmentService service, PredictionService predictionService)
    {
        _service = service;
        _predictionService = predictionService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var shipment = _service.GetById(id);
        if (shipment == null) return NotFound();
        return Ok(shipment);
    }

    [HttpPost]
    public IActionResult Create(CreateShipmentDto dto){
    var shipment = new Shipment
    {
        Origin = dto.Origin,
        Destination = dto.Destination,
        Status = "Created",
        Carrier = "Default Carrier",
        TrackingNumber = Guid.NewGuid().ToString(),
        ExpectedDeliveryDate = DateTime.Now.AddDays(3)
    };

    var prediction = _predictionService.PredictDelay();

    return Ok(new
    {
        shipment,
        prediction
    });
}

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return Ok();
    }
}