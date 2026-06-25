using System.Security.Claims;
using LogisticsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationController(INotificationService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetMine()
    {
        if (!TryGetUserId(out int userId))
            return Unauthorized();

        return Ok(_service.GetForUser(userId));
    }

    [HttpGet("unread-count")]
    public IActionResult GetUnreadCount()
    {
        if (!TryGetUserId(out int userId))
            return Unauthorized();

        return Ok(new { count = _service.GetUnreadCount(userId) });
    }

    [HttpPut("{id}/read")]
    public IActionResult MarkAsRead(int id)
    {
        if (!TryGetUserId(out int userId))
            return Unauthorized();

        var found = _service.MarkAsRead(id, userId);
        return found ? NoContent() : NotFound();
    }

    [HttpPut("mark-all-read")]
    public IActionResult MarkAllAsRead()
    {
        if (!TryGetUserId(out int userId))
            return Unauthorized();

        _service.MarkAllAsRead(userId);
        return NoContent();
    }

    private bool TryGetUserId(out int userId)
    {
        var rawId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? User.FindFirstValue("sub")
                 ?? User.FindFirstValue("nameid");

        return int.TryParse(rawId, out userId);
    }
}
