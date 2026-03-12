using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebApplicationProject.Data;
using WebApplicationProject.Models;

namespace WebApplicationProject.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(ApplicationDbContext db, ILogger<NotificationService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Notification Create(int recipientUserId, string type, string title, string message, string url = null, string data = null)
        {
            var n = new Notification
            {
                RecipientUserId = recipientUserId,
                Type = type ?? string.Empty,
                Title = title ?? string.Empty,
                Message = message ?? string.Empty,
                Url = url ?? "#",
                Data = data ?? string.Empty
            };
            _db.Notifications.Add(n);
            _db.SaveChanges();
            return n;
        }

        public bool TryCreate(int recipientUserId, string type, string title, string message, out string error, string url = null, string data = null)
        {
            error = null;
            try
            {
                Create(recipientUserId, type, title, message, url, data);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Notification create failed for recipient {Recipient}", recipientUserId);
                error = ex.GetBaseException()?.Message ?? ex.Message;
                return false;
            }
        }

        public int GetUnreadCount(int userId)
        {
            try
            {
                return _db.Notifications.Count(n => n.RecipientUserId == userId && !n.IsRead);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "GetUnreadCount failed for user {UserId}", userId);
                return 0;
            }
        }

        public List<Notification> GetRecent(int userId, int limit = 10)
        {
            return _db.Notifications
                .Where(n => n.RecipientUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .ToList();
        }
    }
}
