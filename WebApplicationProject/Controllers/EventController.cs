using Microsoft.AspNetCore.Mvc;
using WebApplicationProject.Models;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using WebApplicationProject.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationProject.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly WebApplicationProject.Services.NotificationService _notiService;

        public EventController(ApplicationDbContext context, WebApplicationProject.Services.NotificationService notiService)
        {
            _context = context;
            _notiService = notiService;
        }
        private int CurrentUserId => HttpContext.Session.GetInt32("UserId") ?? 0;
        public IActionResult Myevent()
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }

            var events = _context.Events.Include(e => e.Participants).ToList();
            var hostIds = events.Select(e => e.UserHostId).Distinct().ToList();
            var hostDict = _context.Users
                                   .Where(u => hostIds.Contains(u.Id))
                                   .ToDictionary(u => u.Id, u => u);

            ViewBag.HostDict = hostDict;
            return View(events);
        }
        public IActionResult Create()
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Event newEvent, IFormFile uploadImage)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            if (newEvent.MaxParticipants < 1)
            {
                TempData["ErrorMessage"] = "จำนวนผู้เข้าร่วมไม่ถูกต้อง";
                return RedirectToAction("Create", "Event");
            }
            int maxAllowedWait = (int)Math.Ceiling(newEvent.MaxParticipants / 2.0);
            if (newEvent.MaxWaiting > maxAllowedWait || newEvent.MaxWaiting < 0)
            {
                return BadRequest("จำนวนตัวสำรองเกินขีดจำกัดที่กำหนดไว้");
            }
            if (newEvent.DateTime < DateTime.Now)
            {
                TempData["ErrorMessage"] = "วันที่ไม่ถูกต้อง";
                return RedirectToAction("Create", "Event");
            }
            if (newEvent.DateTime > newEvent.EndDateTime)
            {
                TempData["ErrorMessage"] = "วันที่ไม่ถูกต้อง";
                return RedirectToAction("Create", "Event");
            }
            string ImageUrl = await UploadImageAsync(uploadImage);
            newEvent.Image = ImageUrl ?? "https://img2.pic.in.th/image-icon-symbol-design-illustration-vector.md.jpg";
            newEvent.Tags = ProcessTags(Request.Form["Tag"]);
            newEvent.UserHostId = CurrentUserId;
            _context.Events.Add(newEvent);
            _context.SaveChanges();
            return RedirectToAction("Manage", new { id = newEvent.Id });
        }

        private bool IsHost(Event ev)
        {
            return ev != null && ev.UserHostId == CurrentUserId;
        }

        public IActionResult Edit(int id)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var eventToEdit = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventToEdit == null) return NotFound("ไม่พบกิจกรรมนี้");
            if (!IsHost(eventToEdit))
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                return RedirectToAction("Myevent");
            }
            if (DateTime.Now >= eventToEdit.DateTime)
            {
                TempData["ErrorMessage"] = "กิจกรรมกำลังดำเนินการหรือจบลงแล้ว ไม่สามารถแก้ไขข้อมูลได้";
                return RedirectToAction("Myevent");
            }
            return View(eventToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Event editEvent, IFormFile uploadImage)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var ogEvent = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == editEvent.Id);
            if (ogEvent != null)
            {
                if(!IsHost(ogEvent))
                {
                    TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                    return RedirectToAction("Myevent");
                }
                if (DateTime.Now >= ogEvent.DateTime)
                {
                    TempData["ErrorMessage"] = "กิจกรรมกำลังดำเนินการหรือจบลงแล้ว ไม่สามารถบันทึกการแก้ไขได้";
                    return RedirectToAction("Myevent");
                }
                if (editEvent.DateTime <= DateTime.Now.AddMinutes(2))
                {
                    TempData["ErrorMessage"] = "วันที่ไม่ถูกต้อง";
                    return RedirectToAction("Create", "Event");
                }
                if (editEvent.DateTime > editEvent.EndDateTime)
                {
                    TempData["ErrorMessage"] = "วันที่ไม่ถูกต้อง";
                    return RedirectToAction("Create", "Event");
                }
                if (editEvent.MaxParticipants < ogEvent.CurrentParticipants || editEvent.MaxParticipants < 1)
                {
                    TempData["ErrorMessage"] = "จำนวนผู้เข้าร่วมไม่ถูกต้อง";
                    return RedirectToAction("Edit", "Event", new { id = editEvent.Id });
                }
                int minRequiredPartiForWait = ogEvent.CurrentWaiting > 0 ? (ogEvent.CurrentWaiting * 2 - 1) : 1;
                if (editEvent.MaxParticipants < minRequiredPartiForWait)
                {
                    TempData["ErrorMessage"] = "จำนวนผู้เข้าร่วมไม่ถูกต้อง";
                    return RedirectToAction("Edit", "Event", new { id = editEvent.Id });
                }
                if (editEvent.MaxWaiting < ogEvent.CurrentWaiting)
                {
                    TempData["ErrorMessage"] = "จำนวนผู้เข้าร่วมไม่ถูกต้อง";
                    return RedirectToAction("Edit", "Event", new { id = editEvent.Id });
                }
                int maxAllowedWait = (int)Math.Ceiling(editEvent.MaxParticipants / 2.0);
                if (editEvent.MaxWaiting > maxAllowedWait || editEvent.MaxWaiting < 0)
                {
                    TempData["ErrorMessage"] = "จำนวนผู้เข้าร่วมไม่ถูกต้อง";
                    return RedirectToAction("Edit", "Event", new { id = editEvent.Id });
                }
                string newImageUrl = await UploadImageAsync(uploadImage);
                if (newImageUrl != null)
                {
                    ogEvent.Image = newImageUrl;
                }
                ogEvent.Title = editEvent.Title;
                ogEvent.Description = editEvent.Description;
                ogEvent.Tags = ProcessTags(Request.Form["Tag"]);
                ogEvent.MaxParticipants = editEvent.MaxParticipants;
                ogEvent.MaxWaiting = editEvent.MaxWaiting;
                ogEvent.DateTime = editEvent.DateTime;
                ogEvent.EndDateTime = editEvent.EndDateTime;
                ogEvent.Location = editEvent.Location;
                _context.SaveChanges();
                // notify participants about event update (excluding editor)
                try
                {
                    var actorName = _context.Users.Where(u => u.Id == CurrentUserId).Select(u => u.Username).FirstOrDefault() ?? "Host";
                    var participantIds = ogEvent.Participants.Select(p => p.UserId).Distinct().Where(id => id != CurrentUserId).ToList();
                    foreach (var pid in participantIds)
                    {
                        _notiService.TryCreate(pid, "event_update", "Event updated", $"{actorName} updated event {ogEvent.Title}", out var _ , $"/Event/Participants/{ogEvent.Id}");
                    }
                }
                catch {
                    TempData["ErrorMessage"] = "Error ไม่สามารถสร้างการแจ้งเตือนไปยังผู้ใช้";
                }
            }
            else
            {
                return NotFound("ไม่พบกิจกรรมนี้");
            }
            return RedirectToAction("Edit", new { id = editEvent.Id });

        }

        public IActionResult Manage(int id)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var eventToManage = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == id);
            if (eventToManage == null) return NotFound("ไม่พบกิจกรรมนี้");
            if (!IsHost(eventToManage))
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                return RedirectToAction("Myevent");
            }
            if (DateTime.Now >= eventToManage.DateTime)
            {
                TempData["ErrorMessage"] = "กิจกรรมเริ่มดำเนินการไปแล้ว ไม่สามารถจัดการผู้เข้าร่วมได้อีก";
                return RedirectToAction("Myevent");
            }
            var participantIds = eventToManage.Participants.Select(p => p.UserId).ToList();
            var participants = _context.Users.Where(u => participantIds.Contains(u.Id)).ToList();
            ViewBag.ParticipantList = participants;
            return View(eventToManage);
        }

        [HttpPost]
        public IActionResult RemoveParti(int eventId, int userId)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var eventToManage = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId);
            if (eventToManage == null) return NotFound("ไม่มีกิจกรรมนี้");
            if (!IsHost(eventToManage))
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                return RedirectToAction("Myevent");
            }
            var ticket = eventToManage.Participants.FirstOrDefault(t => t.UserId == userId);
            if (ticket == null) return NotFound("ไม่มีผู้ใช้นี้ในกิจกรรม");
            ticket.Status = ParticipationStatus.Remove;
            eventToManage.CurrentParticipants = eventToManage.Participants.Count(p => p.Status == ParticipationStatus.Confirmed);
            _context.SaveChanges();
            try
            {
                _notiService.TryCreate(userId, "removed", "Removed from list", $"You were removed from list of {eventToManage.Title}", out var _, "/Event/Myevent");
            }
            catch
            {

            }
            return Ok();
        }
        [HttpPost]
        public IActionResult PromoteParti(int eventId, int userId)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var eventToManage = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId);
            if (eventToManage == null) return NotFound("ไม่มีกิจกรรมนี้");
            if (!IsHost(eventToManage))
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                return RedirectToAction("Myevent");
            }
            if (eventToManage.CurrentParticipants >= eventToManage.MaxParticipants) return BadRequest("ผู้เข้าร่วมตัวจริงเต็มแล้ว ไม่สามารถเพิ่มได้");
            var ticket = eventToManage.Participants.FirstOrDefault(t => t.UserId == userId);
            if (ticket == null) return NotFound("ไม่มีผู้ใช้นี้ในกิจกรรม");
            ticket.Status = ParticipationStatus.Confirmed;
            eventToManage.CurrentParticipants = eventToManage.Participants.Count(p => p.Status == ParticipationStatus.Confirmed);
            eventToManage.CurrentWaiting = eventToManage.Participants.Count(p => p.Status == ParticipationStatus.Waiting);
            _context.SaveChanges();
            try
            {
                _notiService.TryCreate(userId, "Promote", "You're in! 🎉", $"your participation in {eventToManage.Title} is now confirmed.", out var _, $"/Event/Participants/{eventToManage.Id}");
            }
            catch
            {

            }
            return Ok();
        }
        private List<string> ProcessTags(string rawTags)
        {
            if (string.IsNullOrEmpty(rawTags))
                return new List<string>();

            return rawTags.Split(',')
                          .Select(t => t.Trim())
                          .Where(t => !string.IsNullOrEmpty(t))
                          .ToList();
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

        [HttpPost]
        public IActionResult RemoveWaiting(int eventId, int userId)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var eventToManage = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId);
            if (eventToManage == null) return NotFound("ไม่มีกิจกรรมนี้");

            var ticket = eventToManage.Participants.FirstOrDefault(t => t.UserId == userId && t.Status == ParticipationStatus.Waiting);
            if (ticket == null) return NotFound("ไม่มีผู้ใช้นี้ในรายชื่อสำรอง");

            ticket.Status = ParticipationStatus.Remove;
            eventToManage.CurrentWaiting = eventToManage.Participants.Count(p => p.Status == ParticipationStatus.Waiting);
            _context.SaveChanges();
            // notify removed user from waiting list
            try
            {
                _notiService.TryCreate(userId, "removed", "Removed from waiting list", $"You were removed from waiting list of {eventToManage.Title}", out var _ , "/Event/Myevent");
            }
            catch {
                
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Join(int eventId)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var ev = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId);
            if (ev == null) return NotFound("ไม่พบกิจกรรมนี้");

            if (ev.IsRegistrationClosed)
            {
                TempData["ErrorMessage"] = "กิจกรรมนี้ปิดรับสมัครแล้ว";
                return RedirectToAction("Home", "Home");
            }
            if (ev.DateTime <= DateTime.Now.AddMinutes(2))
            {
                TempData["ErrorMessage"] = "กิจกรรมนี้เริ่มไปแล้ว ไม่สามารถเข้าร่วมได้ครับ";
                return RedirectToAction("Home", "Home");
            }

            int userId = CurrentUserId;
            var existing = ev.Participants.FirstOrDefault(p => p.UserId == userId);

            if (existing != null)
            {
                if (existing.Status == ParticipationStatus.Remove)
                {
                    TempData["ErrorMessage"] = "คุณถูกนำออกจากกิจกรรมนี้และไม่สามารถเข้าร่วมซ้ำได้ครับ";
                    return RedirectToAction("Home", "Home");
                }

                if (existing.Status == ParticipationStatus.Confirmed || existing.Status == ParticipationStatus.Waiting)
                {
                    TempData["ErrorMessage"] = "คุณเข้าร่วมกิจกรรมนี้ไปแล้วครับ";
                    return RedirectToAction("Home", "Home");
                }
            }

            bool hasTimeConflict = _context.Events
                .Include(e => e.Participants)
                .Any(otherEvent =>
                otherEvent.Id != ev.Id &&
                otherEvent.DateTime < ev.EndDateTime &&
                otherEvent.EndDateTime > ev.DateTime &&
                otherEvent.Participants.Any(p =>
                    p.UserId == userId &&
                    p.Status == ParticipationStatus.Confirmed)
            );

            if (hasTimeConflict)
            {
                TempData["ErrorMessage"] = "คุณมีกิจกรรมอื่นในเวลาเดียวกันแล้วครับ";
                return RedirectToAction("Home", "Home");
            }

            ParticipationStatus status;

            if (ev.CurrentParticipants < ev.MaxParticipants)
            {
                status = ParticipationStatus.Confirmed;
                ev.CurrentParticipants++;
            }
            else if (ev.CurrentWaiting < ev.MaxWaiting)
            {
                status = ParticipationStatus.Waiting;
                ev.CurrentWaiting++;
            }
            else
            {
                TempData["ErrorMessage"] = "กิจกรรมนี้เต็มแล้วทั้งตัวจริงและตัวสำรอง";
                return RedirectToAction("Home", "Home");
            }

            if (existing != null && existing.Status == ParticipationStatus.Cancelled)
            {
                existing.Status = status;
                existing.JoinedAt = DateTime.Now;
            }
            else
            {
                ev.Participants.Add(new EventParticipation
                {
                    EventId = ev.Id,
                    UserId = userId,
                    Status = status,
                    JoinedAt = DateTime.Now
                });
            }

            ev.CurrentParticipants = ev.Participants.Count(p => p.Status == ParticipationStatus.Confirmed);
            ev.CurrentWaiting = ev.Participants.Count(p => p.Status == ParticipationStatus.Waiting);
            _context.SaveChanges();
            if (status == ParticipationStatus.Confirmed)
            {
                TempData["SuccessMessage"] = "🎉 เข้าร่วมกิจกรรมสำเร็จ! คุณได้สิทธิ์เป็น 'ตัวจริง'";
            }
            else
            {
                TempData["SuccessMessage"] = "📝 เข้าร่วมกิจกรรมสำเร็จ! คุณอยู่ในรายชื่อ 'ตัวสำรอง' โปรดรอการยืนยันจากโฮสต์";
            }
            // notify host that a user joined
            try
            {
                if (ev.UserHostId != userId)
                {
                    var username = _context.Users.Where(u => u.Id == userId).Select(u => u.Username).FirstOrDefault() ?? "Someone";
                    _notiService.TryCreate(ev.UserHostId, "join", "New participant", $"{username} joined {ev.Title}", out var _ , $"/Event/Manage/{ev.Id}");
                }
            }
            catch { }
            return RedirectToAction("Myevent");
        }

        [HttpPost]
        public IActionResult CancelJoin(int eventId)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }
            var ev = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId);
            if (ev == null) return NotFound("ไม่พบกิจกรรมนี้");
            if (ev.IsRegistrationClosed)
            {
                TempData["ErrorMessage"] = "กิจกรรมนี้ปิดรับสมัครและสรุปยอดแล้ว ไม่สามารถยกเลิกได้ครับ";
                return RedirectToAction("Myevent");
            }
            int userId = CurrentUserId;
            var ticket = ev.Participants.FirstOrDefault(p => p.UserId == userId && p.Status != ParticipationStatus.Remove);

            if (ticket != null)
            {
                ticket.Status = ParticipationStatus.Cancelled;
                ev.CurrentWaiting = ev.Participants.Count(p => p.Status == ParticipationStatus.Waiting);
                ev.CurrentParticipants = ev.Participants.Count(p => p.Status == ParticipationStatus.Confirmed);
                _context.SaveChanges();
                // notify host that user canceled
                try
                {
                    var username = _context.Users.Where(u => u.Id == userId).Select(u => u.Username).FirstOrDefault() ?? "Someone";
                    if (ev.UserHostId != userId)
                    {
                        _notiService.TryCreate(ev.UserHostId, "cancel", "Participant canceled", $"{username} canceled their participation in {ev.Title}", out var _ , $"/Event/Manage/{ev.Id}");
                    }
                }
                catch { }
            }

            return RedirectToAction("Myevent");
        }

        [HttpPost]
        public IActionResult ToggleRegistration(int id)
        {
            var ev = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == id);
            if (ev == null) return Json(new { success = false, message = "ไม่พบกิจกรรมนี้" });

            if (!IsHost(ev)) return Json(new { success = false, message = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้" });

            ev.IsRegistrationClosed = !ev.IsRegistrationClosed;
            _context.SaveChanges();

            if (ev.IsRegistrationClosed)
            {
                try
                {
                    var actorName = _context.Users.Where(u => u.Id == CurrentUserId).Select(u => u.Username).FirstOrDefault() ?? "Host";
                    var participantIds = ev.Participants.Select(p => p.UserId).Distinct().Where(pid => pid != CurrentUserId).ToList();
                    foreach (var pid in participantIds)
                    {
                        _notiService.TryCreate(pid, "event_closed", "Registration closed", $"{actorName} closed registration for {ev.Title}", out var _, $"/Event/Participants/{ev.Id}");
                    }
                }
                catch
                {
                    
                }
            }

            return Json(new { success = true, isClosed = ev.IsRegistrationClosed });
        }
        [HttpGet]
        public IActionResult CheckStatus(int id)
        {
            var ev = _context.Events.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();
            return Json(new
            {
                isRegistrationClosed = ev.IsRegistrationClosed,
                dateTime = ev.DateTime
            });
        }


        [HttpGet]
        public IActionResult Participants(int id)
        {
            if (CurrentUserId == 0)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ login เข้าสู่ระบบ";
                return RedirectToAction("Login", "Account");
            }

            var ev = _context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound("ไม่พบกิจกรรมนี้");

            bool isHost = IsHost(ev);
            bool isConfirmed = ev.Participants.Any(p => p.UserId == CurrentUserId && p.Status == ParticipationStatus.Confirmed);

            if (!isHost && !isConfirmed)
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์ดูรายชื่อ เนื่องจากยังไม่ได้เป็นผู้เข้าร่วมตัวจริงครับ";
                return RedirectToAction("Myevent");
            }

            var confirmedParticipantIds = ev.Participants
                .Where(p => p.Status == ParticipationStatus.Confirmed)
                .Select(p => p.UserId)
                .ToList();

            var participants = _context.Users.Where(u => confirmedParticipantIds.Contains(u.Id)).ToList();
            ViewBag.ParticipantList = participants;

            return View(ev);
        }

    }
}