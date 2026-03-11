using Microsoft.AspNetCore.Mvc;
using WebApplicationProject.Data;
using WebApplicationProject.Models;
using System.Linq;

namespace WebApplicationProject.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult UnreadCount()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Json(new { success = false, count = 0 });
            var count = _context.Notifications.Count(n => n.RecipientUserId == userId.Value && !n.IsRead);
            return Json(new { success = true, count = count });
        }

        [HttpGet]
        public JsonResult Recent(int limit = 5)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Json(new { success = false, items = new object[0] });
            var items = _context.Notifications
                .Where(n => n.RecipientUserId == userId.Value)
                .OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .Select(n => new { n.Id, n.Title, n.Message, n.Url, createdAt = n.CreatedAt })
                .ToList();
            return Json(new { success = true, items = items });
        }

        // List notifications for current user
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var notis = _context.Notifications
                .Where(n => n.RecipientUserId == userId.Value)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return View(notis);
        }

        [HttpPost]
        public IActionResult MarkRead(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Unauthorized();

            var n = _context.Notifications.FirstOrDefault(x => x.Id == id && x.RecipientUserId == userId.Value);
            if (n == null) return NotFound();
            n.IsRead = true;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult MarkAllRead()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Unauthorized();

            var items = _context.Notifications.Where(x => x.RecipientUserId == userId.Value && !x.IsRead).ToList();
            foreach (var it in items) it.IsRead = true;
            _context.SaveChanges();
            return Ok();
        }

        // simple API to create a notification (for testing/seed)
        [HttpPost]
        public IActionResult Create(int recipientUserId, string type, string title, string message, string url)
        {
            var n = new Notification
            {
                RecipientUserId = recipientUserId,
                Type = type,
                Title = title,
                Message = message,
                Url = url
            };
            _context.Notifications.Add(n);
            _context.SaveChanges();
            return Ok(n);
        }
    }
}
