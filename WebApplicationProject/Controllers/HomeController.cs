using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplicationProject.Models;
using WebApplicationProject.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Home()
        {
            var events = _context.Events.Include(e => e.Participants).ToList();
            var users = _context.Users.ToList();
            ViewBag.Users = users;
            return View(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult SeedMockData()
        {
            //เช็คข้อมูล
            if (_context.Users.Any())
            {
                return Content("⚠️ Database มีข้อมูลอยู่แล้วครับ! ไม่สามารถเสกซ้ำได้ ป้องกันข้อมูลเบิ้ลครับ");
            }

            //จำ Id เก่าจาก Mock คือ Id ใหม่เบอร์อะไรใน DB
            var userIdMap = new Dictionary<int, int>();
            var eventIdMap = new Dictionary<int, int>();

            // Users
            foreach (var mUser in MockDB.UsersList)
            {
                var newUser = new User
                {
                    Username = mUser.Username,
                    Password = mUser.Password,
                    Email = mUser.Email ?? "mock@email.com",
                    FName = mUser.FName,
                    SName = mUser.SName,
                    Image = mUser.Image,
                    Gender = mUser.Gender,
                    Birthday = mUser.Birthday,
                    Settings = new UserSettings
                    {
                        PrivateAccount = mUser.Settings?.PrivateAccount ?? false,
                        ShowEmail = mUser.Settings?.ShowEmail ?? true,
                        ShowJoinedEvents = mUser.Settings?.ShowJoinedEvents ?? true,
                        ShowHostedEvents = mUser.Settings?.ShowHostedEvents ?? true
                    }
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                userIdMap[mUser.Id] = newUser.Id;
            }

            // Events และ Participants
            foreach (var mEvent in MockDB.EventList)
            {
                var newEvent = new Event
                {
                    Title = mEvent.Title,
                    Description = mEvent.Description,
                    Image = mEvent.Image,
                    Tags = mEvent.Tags,
                    MaxParticipants = mEvent.MaxParticipants,
                    MaxWaiting = mEvent.MaxWaiting,
                    DateTime = mEvent.DateTime,
                    EndDateTime = mEvent.EndDateTime,
                    RegistrationDeadline = mEvent.RegistrationDeadline,
                    Location = mEvent.Location,
                    UserHostId = userIdMap.ContainsKey(mEvent.UserHostId) ? userIdMap[mEvent.UserHostId] : userIdMap.Values.First(),
                    CurrentParticipants = mEvent.CurrentParticipants,
                    CurrentWaiting = mEvent.CurrentWaiting
                };

                if (mEvent.Participants != null)
                {
                    foreach (var mParti in mEvent.Participants)
                    {
                        if (userIdMap.ContainsKey(mParti.UserId))
                        {
                            newEvent.Participants.Add(new EventParticipation
                            {
                                UserId = userIdMap[mParti.UserId],
                                Status = mParti.Status,
                                JoinedAt = mParti.JoinedAt
                            });
                        }
                    }
                }

                _context.Events.Add(newEvent);
                _context.SaveChanges();
                eventIdMap[mEvent.Id] = newEvent.Id;
            }

            foreach (var mUser in MockDB.UsersList)
            {
                if (mUser.Reviewslist != null && mUser.Reviewslist.Any())
                {
                    var targetUserId = userIdMap[mUser.Id];
                    var dbUser = _context.Users.Include(u => u.Reviewslist).First(u => u.Id == targetUserId);

                    foreach (var mReview in mUser.Reviewslist)
                    {
                        if (eventIdMap.ContainsKey(mReview.EventId) && userIdMap.ContainsKey(mReview.UserId))
                        {
                            dbUser.Reviewslist.Add(new Review
                            {
                                EventId = eventIdMap[mReview.EventId],
                                UserId = userIdMap[mReview.UserId],
                                stars = mReview.stars,
                                reviewtitle = mReview.reviewtitle,
                                reviewbody = mReview.reviewbody,
                                IsAnonymous = mReview.IsAnonymous
                            });
                        }
                    }
                }
            }
            _context.SaveChanges();

            return Content("🎉 เสกข้อมูลจาก MockDB ลง Database เรียบร้อยแล้วครับ! กลับไปหน้าเว็บแล้วล็อกอินได้เลย!");
        }

        [HttpGet]
        public IActionResult ClearDatabase()
        {
            _context.Reviews.RemoveRange(_context.Reviews);
            _context.Events.SelectMany(e => e.Participants).ToList().Clear();
            _context.Events.RemoveRange(_context.Events);
            _context.Users.RemoveRange(_context.Users);

            _context.SaveChanges();

            return Content("🗑️ ล้างข้อมูลทุกอย่างใน Database เกลี้ยงแล้วครับ! เป็นฐานข้อมูลว่างเปล่าแล้ว");
        }
    }
}
