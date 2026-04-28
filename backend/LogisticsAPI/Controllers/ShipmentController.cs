using Microsoft.AspNetCore.Mvc;
using LogisticsAPI.Models;
using LogisticsAPI.Services;

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
    public async Task<IActionResult> Create(CreateShipmentDto dto)
    {
        var shipment = _service.Create(dto);

        var prediction = await _predictionService.PredictDelay(new DTOs.PredictionRequestDto
        {
            Origin = dto.Origin,
            Destination = dto.Destination,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate
        });

        return Ok(new { shipment, prediction });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateStatus(int id, UpdateShipmentStatusDto dto)
    {
        _service.UpdateStatus(id, dto.Status);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return Ok();
    }
}