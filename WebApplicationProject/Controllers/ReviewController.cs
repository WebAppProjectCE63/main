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
        private readonly WebApplicationProject.Services.NotificationService _notiService;

        public ReviewController(ApplicationDbContext context, WebApplicationProject.Services.NotificationService notiService)
        {
            _context = context;
            _notiService = notiService;
        }
        private int CurrentUserId => HttpContext.Session.GetInt32("UserId") ?? 0;
        public IActionResult Review(int id)
        {
            // ดึงข้อมูล Event
            var ev = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == id);
            if (ev == null) 
            {
                TempData["ErrorMessage"] = "ไม่พบกิจกรรมดังกล่าว";
                return RedirectToAction("Myevent", "Event");
            }

            int currentUserId = CurrentUserId;
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }

            bool isHost = ev.UserHostId == currentUserId;
            bool isConfirmedParticipant = ev.Participants.Any(p => p.UserId == currentUserId && p.Status == ParticipationStatus.Confirmed);

            if (!isHost && !isConfirmedParticipant)
            {
                TempData["ErrorMessage"] = "คุณไม่เกี่ยวข้องกับกิจกรรมดังกล่าว";
                return RedirectToAction("Myevent", "Event");
            }

            var viewModel = new ReviewEventViewModel
            {
                EventData = ev,
                CurrentLoggedInUserId = currentUserId,
                TargetUsers = new List<ReviewTargetUserModel>()
            };

            // ดึงรายชื่อคนเข้าร่วม (ไม่รวมตัวเอง)
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
                        JoinedAt = DateTime.MinValue,
                        MyReviewToThisUser = existingReview
                    });
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitReview(int EventId, int UserId, int stars, string reviewtitle, int TargetUserId, string reviewbody, bool showname)
        {
            var existing = _context.Reviews.FirstOrDefault(r =>
                                r.EventId == EventId &&
                                r.UserId == UserId &&
                                r.TargetUserId == TargetUserId);
            bool isNew = false;
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
                    TargetUserId = TargetUserId,
                    stars = stars,
                    reviewtitle = reviewtitle,
                    reviewbody = reviewbody,
                    IsAnonymous = showname
                };
                _context.Reviews.Add(newReview);
                isNew = true;
            }
            _context.SaveChanges();

            var actorName = _context.Users.Where(u => u.Id == UserId).Select(u => u.Username).FirstOrDefault() ?? "Someone";
            if (showname)
            {
                actorName = "Anonymous";
            }
            var evt = _context.Events.Find(EventId);
            var title = isNew ? $"New review from {actorName}" : $"Updated review from {actorName}";
            var message = reviewtitle;
            var url = evt != null ? $"/Profile/ProfilePage/{TargetUserId}" : "/";

            if (!_notiService.TryCreate(TargetUserId, "review", title, message, out var notiError, url))
            {
                TempData["NotificationError"] = "Notification create failed: " + notiError;
            }
            return RedirectToAction("Review", new { id = EventId });
        }
    }
}
