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
        // 2. จำลองข้อมูล User (ฉบับอัปเดต เพิ่ม Gender & Birthday)
        static List<User> Users = new List<User>()
        {
            // 👑 1. Host งานดนตรี (ID: 101) - ผู้ชาย วัยทำงาน
            new User
            {
                Id = 101,
                Username = "music_host",
                Password = "123",
                FName = "ก้องเกียรติ",
                SName = "ใจดี",
                Email = "kong@music.com",
                Gender = "Male", // ✅ เพิ่มเพศ
                Birthday = new DateTime(1990, 5, 20), // ✅ เพิ่มวันเกิด (อายุ 36)
                Image = "https://ui-avatars.com/api/?name=Kong+J&background=random&size=128",
                MyEvents = new List<EventParticipation>()
            },

            // 👑 2. Host งาน Workshop (ID: 102) - ผู้หญิง ศิลปิน
            new User
            {
                Id = 102,
                Username = "art_host",
                Password = "123",
                FName = "ปั้นจั่น",
                SName = "งานละเอียด",
                Email = "pun@art.com",
                Gender = "Female", // ✅ เพิ่มเพศ
                Birthday = new DateTime(1995, 8, 15), // ✅ เพิ่มวันเกิด (อายุ 31)
                Image = "https://ui-avatars.com/api/?name=Pun+N&background=random&size=128",
                MyEvents = new List<EventParticipation>()
            },

            // 👤 3. สมชาย (ID: 103) - ผู้ชาย (ตัวจริงทุกงาน)
            new User
            {
                Id = 103,
                Username = "somchai",
                Password = "123",
                FName = "สมชาย",
                SName = "เข็มกลัด",
                Email = "somchai@test.com",
                Gender = "Male", // ✅ เพิ่มเพศ
                Birthday = new DateTime(1985, 1, 1), // ✅ เพิ่มวันเกิด (อายุ 41)
                Image = "https://ui-avatars.com/api/?name=Somchai+K&background=0D8ABC&color=fff&size=128",
                MyEvents = new List<EventParticipation>()
            },

            // 👤 4. แนนซี่ (ID: 104) - ผู้หญิง (ตัวสำรองงานดนตรี)
            new User
            {
                Id = 104,
                Username = "nancy",
                Password = "123",
                FName = "แนนซี่",
                SName = "มีตังค์",
                Email = "nancy@test.com",
                Gender = "Female", // ✅ เพิ่มเพศ
                Birthday = new DateTime(2000, 12, 25), // ✅ เพิ่มวันเกิด (วัยรุ่น อายุ 25)
                Image = "https://ui-avatars.com/api/?name=Nancy+M&background=FFC107&size=128",
                MyEvents = new List<EventParticipation>()
            },

            // 👤 5. ปิติ (ID: 105) - ผู้ชาย (ตัวจริงงานดนตรี)
            new User
            {
                Id = 105,
                Username = "piti",
                Password = "123",
                FName = "ปิติ",
                SName = "พอใจ",
                Email = "piti@test.com",
                Gender = "Male", // ✅ เพิ่มเพศ
                Birthday = new DateTime(1998, 3, 10), // ✅ เพิ่มวันเกิด (อายุ 28)
                Image = "https://ui-avatars.com/api/?name=Piti+P&background=8E44AD&color=fff&size=128",
                MyEvents = new List<EventParticipation>()
            },

            // 👤 6. ชูใจ (ID: 106) - LGBTQ+ (ตัวสำรองงาน Workshop)
            new User
            {
                Id = 106,
                Username = "chujai",
                Password = "123",
                FName = "ชูใจ",
                SName = "เลิศล้ำ",
                Email = "chujai@test.com",
                Gender = "LGBTQ+", // ✅ เพิ่มเพศทางเลือก
                Birthday = new DateTime(1992, 11, 5), // ✅ เพิ่มวันเกิด (อายุ 33)
                Image = "https://ui-avatars.com/api/?name=Chujai+L&background=E74C3C&color=fff&size=128",
                MyEvents = new List<EventParticipation>()
            }
        };
        public IActionResult Myevent()
        {
            return View(EventStore.Events);
        }
        public IActionResult Create()   
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Event newEvent, IFormFile uploadImage)
        {
            string ImageUrl = await UploadImageAsync(uploadImage);
            newEvent.Image = ImageUrl ?? "https://img2.pic.in.th/image-icon-symbol-design-illustration-vector.md.jpg";
            newEvent.Tags = ProcessTags(Request.Form["Tag"]);
            newEvent.UserHostId = 101;
            newEvent.Id = EventStore.Events.Count + 1;
            EventStore.Events.Add(newEvent);
            return RedirectToAction("Create");
        }

        public IActionResult Edit(int id)
        {

            var eventToEdit = EventStore.Events.FirstOrDefault(e => e.Id == id);
            if (eventToEdit == null) return NotFound();
            return View(eventToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Event editEvent, IFormFile uploadImage)
        {
            var ogEvent = EventStore.Events.FirstOrDefault(e => e.Id == editEvent.Id);
            if (ogEvent != null)
            {
                string newImageUrl = await UploadImageAsync(uploadImage);
                if (newImageUrl != null)
                {
                    ogEvent.Image = newImageUrl;
                }
                ogEvent.Title = editEvent.Title;
                ogEvent.Description = editEvent.Description;
                ogEvent.Tags = ProcessTags(Request.Form["Tag"]);
                ogEvent.MaxParticipants = editEvent.MaxParticipants;
                ogEvent.DateTime = editEvent.DateTime;
                ogEvent.Location = editEvent.Location;
            }
            return RedirectToAction("Edit", new {id = editEvent.Id});

        }

        public IActionResult Manage(int id)
        {
            var eventToManage = EventStore.Events.FirstOrDefault(e => e.Id == id);
            if (eventToManage == null) return NotFound();
            var participantIds = eventToManage.Participants.Select(p => p.UserId).ToList();
            var participants = Users.Where(u => participantIds.Contains(u.Id)).ToList();
            ViewBag.ParticipantList = participants;
            return View(eventToManage);
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

            return null; // ถ้าอัปโหลดล้มเหลว
        }
    }
}
