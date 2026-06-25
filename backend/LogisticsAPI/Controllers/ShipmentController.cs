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

    private readonly INotificationService _notificationService;  


    public ShipmentController(ShipmentService service, PredictionService predictionService, INotificationService notificationService)
    {
        _service = service;
        _predictionService = predictionService;
        _notificationService = notificationService;
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
    if (!TryGetUserId(out int userId)) return Unauthorized();

    var shipment = await _service.CreateAsync(dto, userId);  // now async

    return CreatedAtAction(nameof(GetMineById), new { id = shipment.Id }, shipment);
}


    [HttpPost("predict")]
    [Authorize]
    public async Task<IActionResult> Predict([FromBody] PredictionRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Origin) || string.IsNullOrWhiteSpace(dto.Destination))
            return BadRequest(new { message = "Origin and destination are required." });

        var result = await _predictionService.PredictDelayAsync(dto);
        if (result.RiskLevel != null &&
        !string.Equals(result.RiskLevel, "low", StringComparison.OrdinalIgnoreCase) &&
        TryGetUserId(out int userId))
    {
        var fakeShipment = new LogisticsAPI.Models.Shipment
        {
            UserId = userId,
            Origin = dto.Origin,
            Destination = dto.Destination,
            ShipmentNumber = "PENDING"
        };
        await _notificationService.NotifyDelayAsync(fakeShipment);
    }
        return Ok(result);
    }

[HttpPut("{id}/status")]
public async Task<IActionResult> UpdateStatus(int id, UpdateShipmentStatusDto dto)
{
    var existing = _service.GetById(id);
    if (existing == null) return NotFound();
    if (!User.IsInRole("Admin"))
    {
        if (!TryGetUserId(out int userId) || existing.UserId != userId)
            return Forbid();
    }
    await _service.UpdateStatusAsync(id, dto.Status);
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