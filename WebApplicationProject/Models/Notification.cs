using System;

namespace WebApplicationProject.Models
{
    public class Notification
    {
        public int Id { get; set; }
        // recipient user id
        public int RecipientUserId { get; set; }

        // simple type: "review", "system", "invite" etc.
        public string Type { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        // optional Url the notification should link to
        public string Url { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        // optional JSON payload
        public string Data { get; set; }
    }
}
