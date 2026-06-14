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
    public async Task<IActionResult> Create(CreateShipmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var shipment = _service.Create(dto);

        var prediction = await _predictionService.PredictDelayAsync(new PredictionRequestDto
        {
            Origin = dto.Origin,
            Destination = dto.Destination,
            Carrier = dto.Carrier ?? "Default Carrier"
        });

        return CreatedAtAction(nameof(GetById), new { id = shipment.Id }, new
        {
            shipment,
            prediction
        });
    }

    [HttpPost("predict")]
    public async Task<IActionResult> Predict([FromBody] PredictionRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Origin) || string.IsNullOrWhiteSpace(dto.Destination))
            return BadRequest(new { message = "Origin and destination are required." });

        var result = await _predictionService.PredictDelayAsync(dto);
        return Ok(result);
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