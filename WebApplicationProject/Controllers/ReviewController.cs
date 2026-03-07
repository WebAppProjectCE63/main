using Microsoft.AspNetCore.Mvc;
using WebApplicationProject.Models;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using WebApplicationProject.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationProject.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        private int CurrentUserId => HttpContext.Session.GetInt32("UserId") ?? 0;
        public IActionResult Review(int id)
        {
            // 1. ดึงข้อมูล Event
            var ev = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();

            int currentUserId = CurrentUserId;

            var viewModel = new ReviewEventViewModel
            {
                EventData = ev,
                CurrentLoggedInUserId = currentUserId,
                TargetUsers = new List<ReviewTargetUserModel>()
            };

            // 2. ดึงรายชื่อคนเข้าร่วม (Confirmed และไม่ใช่ตัวเอง)
            var participants = ev.Participants
                .Where(p => p.Status == ParticipationStatus.Confirmed && p.UserId != currentUserId)
                .OrderBy(p => p.JoinedAt)
                .ToList();

            var participantIds = participants.Select(p => p.UserId).ToList();
            if (!participantIds.Contains(ev.UserHostId)) participantIds.Add(ev.UserHostId);

            var relatedUsers = _context.Users
                .Include(u => u.Reviewslist)
                .Where(u => participantIds.Contains(u.Id))
                .ToList();

            foreach (var p in participants)
            {
                var user = relatedUsers.FirstOrDefault(u => u.Id == p.UserId);
                if (user != null)
                {
                    var existingReview = user.Reviewslist?.FirstOrDefault(r => r.EventId == ev.Id && r.UserId == currentUserId);
                    viewModel.TargetUsers.Add(new ReviewTargetUserModel
                    {
                        UserInfo = user,
                        IsHost = false,
                        JoinedAt = p.JoinedAt,
                        MyReviewToThisUser = existingReview
                    });
                }
            }

            // 3. ดึงข้อมูล Host (ถ้า Host ไม่ใช่ตัวเอง และยังไม่อยู่ในลิสต์เข้าร่วมด้านบน)
            if (ev.UserHostId != currentUserId && !viewModel.TargetUsers.Any(t => t.UserInfo.Id == ev.UserHostId))
            {
                var hostUser = relatedUsers.FirstOrDefault(u => u.Id == ev.UserHostId);
                if (hostUser != null)
                {
                    var existingReview = hostUser.Reviewslist?.FirstOrDefault(r => r.EventId == ev.Id && r.UserId == currentUserId);
                    viewModel.TargetUsers.Add(new ReviewTargetUserModel
                    {
                        UserInfo = hostUser,
                        IsHost = true,
                        JoinedAt = DateTime.MinValue, // หรือตั้งเป็นเวลาสร้าง Event ก็ได้
                        MyReviewToThisUser = existingReview
                    });
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitReview(int EventId, int UserId, int stars, string reviewtitle, int TargetUserId, string reviewbody, bool showname)
        {
            var targetUser = _context.Users.Include(u => u.Reviewslist).FirstOrDefault(u => u.Id == TargetUserId);
            if (targetUser == null) return NotFound();

            var existing = targetUser.Reviewslist.FirstOrDefault(r => r.EventId == EventId && r.UserId == UserId);
            if (existing != null)
            {
                existing.stars = stars;
                existing.reviewtitle = reviewtitle;
                existing.reviewbody = reviewbody;
                existing.IsAnonymous = showname;
            }
            else
            {
                var newReview = new Review
                {
                    EventId = EventId,
                    UserId = UserId,
                    stars = stars,
                    reviewtitle = reviewtitle,
                    reviewbody = reviewbody,
                    IsAnonymous = showname
                };
                targetUser.Reviewslist.Add(newReview);
            }
            _context.SaveChanges();
            return RedirectToAction("Review", new { id = EventId });
        }
    }
}
