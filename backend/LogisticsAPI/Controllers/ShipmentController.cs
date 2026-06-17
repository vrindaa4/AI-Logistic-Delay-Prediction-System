using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LogisticsAPI.Models;
using LogisticsAPI.Services;
using LogisticsAPI.DTOs;
using System.Security.Claims;
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

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("my")]
    public IActionResult GetMine()
    {
        if (!TryGetUserId(out int userId))
            return Unauthorized();

        return Ok(_service.GetByUserId(userId));
    }

    [HttpGet("my/{id}")]
    public IActionResult GetMineById(int id)
    {
        if (!TryGetUserId(out int userId))
            return Unauthorized();

        var shipment = _service.GetByIdForUser(id, userId);
        if (shipment == null) return NotFound();
        return Ok(shipment);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var shipment = _service.GetById(id);
        if (shipment == null) return NotFound();
        return Ok(shipment);
    }

    private bool TryGetUserId(out int userId)
    {
        var rawId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? User.FindFirstValue("sub")
                 ?? User.FindFirstValue("nameid");

        return int.TryParse(rawId, out userId);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateShipmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!TryGetUserId(out int userId))
            return Unauthorized();

        var shipment = _service.Create(dto, userId);

        var prediction = await _predictionService.PredictDelayAsync(new PredictionRequestDto
        {
            Origin = dto.Origin,
            Destination = dto.Destination,
            Carrier = dto.Carrier ?? "Default Carrier"
        });

        return CreatedAtAction(nameof(GetMineById), new { id = shipment.Id }, new
        {
            shipment,
            prediction
        });
    }

    [AllowAnonymous]
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

        if (!User.IsInRole("Admin"))
        {
            if (!TryGetUserId(out int userId) || existing.UserId != userId)
                return Forbid();
        }

        _service.UpdateStatus(id, dto.Status);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _service.GetById(id);
        if (existing == null) return NotFound();

        if (!User.IsInRole("Admin"))
        {
            if (!TryGetUserId(out int userId) || existing.UserId != userId)
                return Forbid();
        }

        _service.Delete(id);
        return Ok();
    }
}