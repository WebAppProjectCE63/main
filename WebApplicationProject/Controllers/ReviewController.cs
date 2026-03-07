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

namespace WebApplicationProject.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Review(int id)
        {
            // 1. ดึงข้อมูล Event
            var ev = MockDB.EventList.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();

            int currentUserId = MockDB.CurrentLoggedInUserId;

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

            foreach (var p in participants)
            {
                var user = MockDB.UsersList.FirstOrDefault(u => u.Id == p.UserId);
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
                var hostUser = MockDB.UsersList.FirstOrDefault(u => u.Id == ev.UserHostId);
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
            var targetUser = MockDB.UsersList.FirstOrDefault(u => u.Id == TargetUserId);
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
                var newId = (targetUser.Reviewslist.Count == 0) ? 1 : targetUser.Reviewslist.Max(r => r.Id) + 1;
                var newReview = new Review
                {
                    Id = newId,
                    EventId = EventId,
                    UserId = UserId,
                    stars = stars,
                    reviewtitle = reviewtitle,
                    reviewbody = reviewbody,
                    IsAnonymous = showname
                };
                targetUser.Reviewslist.Add(newReview);
            }
            return RedirectToAction("Review", new { id = EventId });
        }
    }
}
