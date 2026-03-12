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
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }
        private int CurrentUserId => HttpContext.Session.GetInt32("UserId") ?? 0;
        public IActionResult profilepage(int? id = null)
        {
            int targetId = id ?? CurrentUserId;
            if (CurrentUserId == 0) {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login","Account");
            }
            User ? currentUser = _context.Users
                .Include(u => u.Reviewslist)
                .FirstOrDefault(u => u.Id == targetId);

            if (currentUser == null) 
            {
                TempData["ErrorMessage"] = "ไม่พบบัญชีดังกล่าว";
                return RedirectToAction("profilepage", new { id = CurrentUserId }); 
            }

            var viewModel = new ProfilePageViewModel
            {
                UserInfo = currentUser,
                CurrentLoggedInUserId = CurrentUserId
            };

            if (currentUser.Settings != null && currentUser.Settings.PrivateAccount && currentUser.Id != CurrentUserId)
            {
                return View(viewModel);
            }
            var allEvents = _context.Events.Include(e => e.Participants).ToList();
            var allUsers = _context.Users.ToList();

            var rawHostedEvents = GetMyHostedEvents(allEvents, targetId);
            viewModel.HostedEvents = rawHostedEvents.Select(ev => new EventDisplayModel
            {
                EventData = ev,
                ParticipantAvatars = allUsers.Where(u => ev.Participants
                    .Where(p => p.Status == ParticipationStatus.Confirmed)
                    .Select(p => p.UserId)
                    .Contains(u.Id))
                    .Take(3)
                    .ToList()
            }).ToList();

            var rawJoinedEvents = allEvents
                .Where(ev => ev.Participants.Any(p => p.UserId == targetId && p.Status == ParticipationStatus.Confirmed))
                .ToList();

            viewModel.JoinedEvents = rawJoinedEvents.Select(ev => new EventDisplayModel
            {
                EventData = ev,
                ParticipantAvatars = allUsers.Where(u => ev.Participants
                    .Where(p => p.Status == ParticipationStatus.Confirmed)
                    .Select(p => p.UserId)
                    .Contains(u.Id))
                    .Take(3)
                    .ToList()
            }).ToList();

            if (currentUser.Reviewslist != null && currentUser.Reviewslist.Any())
            {
                viewModel.Reviews = currentUser.Reviewslist.Select(r => new ReviewDisplayModel
                {
                    ReviewData = r,
                    Author = allUsers.FirstOrDefault(u => u.Id == r.UserId),
                    EventTitle = allEvents.FirstOrDefault(e => e.Id == r.EventId)?.Title ?? "Unknown Event"
                }).ToList();
            }

            return View(viewModel);
        }

        private List<Event> GetMyHostedEvents(List<Event> allEvents, int UserID)
            {
                if (allEvents == null) return new List<Event>();
                return allEvents.Where(ev => ev.UserHostId == UserID).ToList();
            }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User editUser, bool IsPublic, bool ShowEmail, bool ShowJoinedEvents, bool ShowHostedEvents, IFormFile uploadImage)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var ogUser = _context.Users.FirstOrDefault(u => u.Id == editUser.Id);
            if (ogUser == null)
            {
                return NotFound();
            }
            if (ogUser != null)
            {
                string newImageUrl = await UploadImageAsync(uploadImage);
                if (newImageUrl != null)
                {
                    ogUser.Image = newImageUrl;
                }

                ogUser.FName = editUser.FName;
                ogUser.SName = editUser.SName;
                ogUser.Email = editUser.Email;
                ogUser.Gender = editUser.Gender;

                if (ogUser.Settings == null)
                {
                    ogUser.Settings = new UserSettings();
                }
                ogUser.Settings.PrivateAccount = IsPublic;
                ogUser.Settings.ShowEmail = ShowEmail;
                ogUser.Settings.ShowHostedEvents = ShowHostedEvents;
                ogUser.Settings.ShowJoinedEvents = ShowJoinedEvents;
            }
            _context.SaveChanges();
            return RedirectToAction("profilepage");
        }

        private async Task<string> UploadImageAsync(IFormFile uploadImage)
        {
            if (uploadImage == null || uploadImage.Length == 0)
                return null;

            using var ms = new MemoryStream();
            await uploadImage.CopyToAsync(ms);
            string base64Image = Convert.ToBase64String(ms.ToArray());

            string apiKey = "d0389bb796bb619e0b8f1503873fbc8a";
            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("image", base64Image)
            });

            var response = await client.PostAsync($"https://api.imgbb.com/1/upload?key={apiKey}", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                return doc.RootElement.GetProperty("data").GetProperty("url").GetString();
            }

            return null;
        }
    }
}