using LogisticsAPI.DTOs;
using LogisticsAPI.Models;

namespace LogisticsAPI.Services;

public interface INotificationService
{   
    Task NotifyDelayAsync(Shipment shipment);
    Task NotifyShipmentCreatedAsync(Shipment shipment);        
    Task NotifyStatusChangedAsync(Shipment shipment, string previousStatus);

    List<NotificationDto> GetForUser(int userId);
    int GetUnreadCount(int userId);
    bool MarkAsRead(int id, int userId);
    void MarkAllAsRead(int userId);
}
