using System;

namespace WebApplicationProject.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int RecipientUserId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public string Data { get; set; }
    }
}
