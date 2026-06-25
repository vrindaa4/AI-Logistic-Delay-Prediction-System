using LogisticsAPI.Models;

namespace LogisticsAPI.Repositories;

public interface INotificationRepository
{
    List<Notification> GetByUserId(int userId, int take = 50);
    int GetUnreadCount(int userId);
    Notification Add(Notification notification);
    Notification? GetByIdForUser(int id, int userId);
    void MarkAsRead(Notification notification);
    void MarkAllAsRead(int userId);
}
