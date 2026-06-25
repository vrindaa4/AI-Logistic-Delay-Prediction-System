using LogisticsAPI.Data;
using LogisticsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAPI.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly LogisticsDbContext _context;

    public NotificationRepository(LogisticsDbContext context)
    {
        _context = context;
    }

    public List<Notification> GetByUserId(int userId, int take = 5)
        => _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAtUtc)
            .Take(take)
            .ToList();

    public int GetUnreadCount(int userId)
        => _context.Notifications.Count(n => n.UserId == userId && !n.IsRead);

    public Notification Add(Notification notification)
    {
        notification.CreatedAtUtc = DateTime.UtcNow;
        _context.Notifications.Add(notification);
        _context.SaveChanges();
        return notification;
    }

    public Notification? GetByIdForUser(int id, int userId)
        => _context.Notifications.FirstOrDefault(n => n.Id == id && n.UserId == userId);

    public void MarkAsRead(Notification notification)
    {
        notification.IsRead = true;
        _context.SaveChanges();
    }

    public void MarkAllAsRead(int userId)
    {
        var unread = _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToList();

        foreach (var n in unread)
            n.IsRead = true;

        _context.SaveChanges();
    }
}
