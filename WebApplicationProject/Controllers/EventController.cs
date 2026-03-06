using Microsoft.AspNetCore.Mvc;
using WebApplicationProject.Models;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using WebApplicationProject.Data;
namespace WebApplicationProject.Controllers

{
    public class EventController : Controller
    {
        public IActionResult Myevent()
        {
            return View(MockDB.EventList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Event newEvent, IFormFile uploadImage)
        {
            if (newEvent.MaxParticipants < 1) return BadRequest("จำนวนผู้เข้าร่วมไม่ถูกต้อง");
            int maxAllowedWait = (int)Math.Ceiling(newEvent.MaxParticipants / 2.0);
            if (newEvent.MaxWaiting > maxAllowedWait || newEvent.MaxWaiting < 0)
            {
                return BadRequest("จำนวนตัวสำรองเกินขีดจำกัดที่กำหนดไว้");
            }
            string ImageUrl = await UploadImageAsync(uploadImage);
            newEvent.Image = ImageUrl ?? "https://img2.pic.in.th/image-icon-symbol-design-illustration-vector.md.jpg";
            newEvent.Tags = ProcessTags(Request.Form["Tag"]);
            newEvent.UserHostId = MockDB.CurrentLoggedInUserId;
            newEvent.Id = MockDB.EventList.Count + 1;
            MockDB.EventList.Add(newEvent);
            return RedirectToAction("Manage", new { id = newEvent.Id });
        }

        private bool IsHost(Event ev)
        {
            return ev != null && ev.UserHostId == MockDB.CurrentLoggedInUserId;
        }

        public IActionResult Edit(int id)
        {
            var eventToEdit = MockDB.EventList.FirstOrDefault(e => e.Id == id);
            if (eventToEdit == null) return NotFound("ไม่พบกิจกรรมนี้");
            if (!IsHost(eventToEdit))
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                return RedirectToAction("Myevent");
            }
            return View(eventToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Event editEvent, IFormFile uploadImage)
        {
            var ogEvent = MockDB.EventList.FirstOrDefault(e => e.Id == editEvent.Id);
            if (ogEvent != null)
            {
                if (!IsHost(ogEvent))
                {
                    TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                    return RedirectToAction("Myevent");
                }
                if (editEvent.MaxParticipants < ogEvent.CurrentParticipants || editEvent.MaxParticipants < 1) return BadRequest("จำนวนผู้เข้าร่วมไม่ถูกต้อง");
                int minRequiredPartiForWait = ogEvent.CurrentWaiting > 0 ? (ogEvent.CurrentWaiting * 2 - 1) : 1;
                if (editEvent.MaxParticipants < minRequiredPartiForWait)
                {
                    return BadRequest($"ไม่สามารถลดจำนวนตัวจริงได้ ต้องตั้งไว้อย่างน้อย {minRequiredPartiForWait} คน เพื่อให้สอดคล้องกับตัวสำรองที่มีอยู่แล้ว ({ogEvent.CurrentWaiting} คน)");
                }
                if (editEvent.MaxWaiting < ogEvent.CurrentWaiting)
                {
                    return BadRequest("ไม่สามารถลดจำนวนรับสำรองให้น้อยกว่าคนที่อยู่ในคิวปัจจุบันได้");
                }
                int maxAllowedWait = (int)Math.Ceiling(editEvent.MaxParticipants / 2.0);
                if (editEvent.MaxWaiting > maxAllowedWait || editEvent.MaxWaiting < 0)
                {
                    return BadRequest("จำนวนตัวสำรองเกินขีดจำกัดที่กำหนดไว้");
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
            }
            else
            {
                return NotFound("ไม่พบกิจกรรมนี้");
            }
            return RedirectToAction("Edit", new { id = editEvent.Id });

        }

        public IActionResult Manage(int id)
        {
            var eventToManage = MockDB.EventList.FirstOrDefault(e => e.Id == id);
            if (eventToManage == null) return NotFound("ไม่พบกิจกรรมนี้");
            if (!IsHost(eventToManage))
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้ เนื่องจากไม่ใช่เจ้าของกิจกรรม";
                return RedirectToAction("Myevent");
            }
            var participantIds = eventToManage.Participants.Select(p => p.UserId).ToList();
            var participants = MockDB.UsersList.Where(u => participantIds.Contains(u.Id)).ToList();
            ViewBag.ParticipantList = participants;
            return View(eventToManage);
        }

        [HttpPost]
        public IActionResult RemoveParti(int eventId, int userId)
        {
            var eventToManage = MockDB.EventList.FirstOrDefault(e => e.Id == eventId);
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
            return Ok();
        }
        [HttpPost]
        public IActionResult PromoteParti(int eventId, int userId)
        {
            var eventToManage = MockDB.EventList.FirstOrDefault(e => e.Id == eventId);
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
            public IActionResult Join(int eventId)
            {
                var ev = MockDB.EventList.FirstOrDefault(e => e.Id == eventId);
                if (ev == null) return NotFound("ไม่พบกิจกรรมนี้");

                int userId = MockDB.CurrentLoggedInUserId;

                if (ev.UserHostId == userId)
                    return BadRequest("Host ไม่ต้องกด Join");

                var existing = ev.Participants.FirstOrDefault(p => p.UserId == userId && p.Status != ParticipationStatus.Remove);
                if (existing != null)
                    return BadRequest("คุณเข้าร่วมกิจกรรมนี้แล้ว");

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

                int newId = (ev.Participants.Count == 0) ? 1 : ev.Participants.Max(p => p.Id) + 1;
                    
                ev.Participants.Add(new EventParticipation
                {
                    Id = newId,
                    EventId = ev.Id,
                    UserId = userId,
                    Status = status,
                    JoinedAt = DateTime.Now
                });

                ev.CurrentParticipants = ev.Participants.Count(p => p.Status == ParticipationStatus.Confirmed);

                return RedirectToAction("Myevent", "Event");
        }
        }
    }
