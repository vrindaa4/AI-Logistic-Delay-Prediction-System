using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LogisticsAPI.Models;
using LogisticsAPI.Services;
using LogisticsAPI.DTOs;
namespace LogisticsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var shipment = _service.Create(dto);

        var prediction = _predictionService.PredictDelay();

        return CreatedAtAction(nameof(GetById), new { id = shipment.Id }, new
        {
            shipment,
            prediction
        });
    }

    [HttpPut("{id}/status")]
    public IActionResult UpdateStatus(int id, UpdateShipmentStatusDto dto)
    {
        var existing = _service.GetById(id);
        if (existing == null) return NotFound();

        _service.UpdateStatus(id, dto.Status);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return Ok();
    }
}