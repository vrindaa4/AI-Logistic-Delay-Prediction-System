using LogisticsAPI.DTOs;
using LogisticsAPI.Hubs;
using LogisticsAPI.Models;
using LogisticsAPI.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsAPI.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly IHubContext<NotificationHub> _hub;
    private readonly ILogger<NotificationService> _logger;


    public NotificationService(
        INotificationRepository repo,
        IHubContext<NotificationHub> hub,
        ILogger<NotificationService> logger)
    {
        _repo = repo;
        _hub = hub;
        _logger = logger;
    }

    public async Task NotifyDelayAsync(Shipment shipment)
    {
  
        if (shipment.UserId is not int userId) return;

        var notification = new Notification
        {
            UserId = userId,
            ShipmentId = shipment.Id,
            ShipmentNumber = shipment.ShipmentNumber,
            Type = "DelayAlert",
            Title = "Shipment Delayed",
            Message = $"Shipment {shipment.ShipmentNumber} ({shipment.Origin} \u2192 {shipment.Destination}) " +
                      "has been marked as delayed."
        };

        var saved = _repo.Add(notification);
        var dto = ToDto(saved);

        try
        {
            await _hub.Clients
                .Group(NotificationHub.GroupName(userId))
                .SendAsync("ReceiveNotification", dto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Delay alert saved but real-time push failed for shipment {ShipmentId}", shipment.Id);
        }
    }

    public async Task NotifyShipmentCreatedAsync(Shipment shipment)
{
    if (shipment.UserId is not int userId) return;

    var notification = new Notification
    {
        UserId = userId,
        ShipmentId = shipment.Id,
        ShipmentNumber = shipment.ShipmentNumber,
        Type = "ShipmentCreated",
        Title = "Shipment Created",
        Message = "Shipment has been created successfully. We will keep you updated about its status."
    };

    var saved = _repo.Add(notification);
    var dto = ToDto(saved);

    try
    {
        await _hub.Clients
            .Group(NotificationHub.GroupName(userId))
            .SendAsync("ReceiveNotification", dto);
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "Shipment created notification push failed for shipment {ShipmentId}", shipment.Id);
    }
}

public async Task NotifyStatusChangedAsync(Shipment shipment, string previousStatus)
{
    if (shipment.UserId is not int userId) return;

    string? message = null;
    string? type = null;
    string? title = null;

    var prev = previousStatus.ToLower();
    var curr = shipment.Status.ToLower();

    if (prev == "pending" && curr == "in-transit")
    {
        message = "Your shipment is now in transit.";
        type = "InTransit";
        title = "Shipment In Transit";
    }
    else if (prev == "in-transit" && curr == "delayed")
    {
        message = "Your shipment has been delayed.";
        type = "Delayed";
        title = "Shipment Delayed";
    }
    else if (curr == "delivered")
    {
        message = "Your shipment has been delivered successfully.";
        type = "Delivered";
        title = "Shipment Delivered";
    }

    if (message == null) return;

    var notification = new Notification
    {
        UserId = userId,
        ShipmentId = shipment.Id,
        ShipmentNumber = shipment.ShipmentNumber,
        Type = type!,
        Title = title!,
        Message = message
    };

    var saved = _repo.Add(notification);
    var dto = ToDto(saved);

    try
    {
        await _hub.Clients
            .Group(NotificationHub.GroupName(userId))
            .SendAsync("ReceiveNotification", dto);
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "Status change notification push failed for shipment {ShipmentId}", shipment.Id);
    }
}

    public List<NotificationDto> GetForUser(int userId)
        => _repo.GetByUserId(userId).Select(ToDto).ToList();

    public int GetUnreadCount(int userId)
        => _repo.GetUnreadCount(userId);

    public bool MarkAsRead(int id, int userId)
    {
        var notification = _repo.GetByIdForUser(id, userId);
        if (notification == null) return false;

        _repo.MarkAsRead(notification);
        return true;
    }

    public void MarkAllAsRead(int userId)
        => _repo.MarkAllAsRead(userId);

    private static NotificationDto ToDto(Notification n) => new()
    {
        Id = n.Id,
        ShipmentId = n.ShipmentId,
        ShipmentNumber = n.ShipmentNumber,
        Title = n.Title,
        Message = n.Message,
        Type = n.Type,
        IsRead = n.IsRead,
        CreatedAtUtc = n.CreatedAtUtc
    };
}
