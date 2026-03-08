using WebApplicationProject.Models;

namespace WebApplicationProject.Data
{
    public class MockDB
    {
        // จำลองว่า User ID 102 (ปั้นจั่น) เป็นคนล็อกอิน (เพื่อให้เป็น Host ของ Event 2 และเป็นผู้เข้าร่วม Event ที่จบไปแล้ว)
        public static int CurrentLoggedInUserId = 102;

        // 1. จำลองข้อมูล Event 
        public static List<Event> EventList = new List<Event>()
        {
            new Event
            {
                Id = 1, Title = "ดนตรีในสวน (Music in the Park)", Description = "มาร่วมฟังดนตรีสดบรรยากาศชิลๆ ยามเย็น", Image = "https://img2.pic.in.th/cover-1.md.jpg", Location = "สวนลุมพินี กรุงเทพฯ", DateTime = DateTime.Now.AddDays(3).Date.AddHours(17), EndDateTime = DateTime.Now.AddDays(3).Date.AddHours(20), Tags = new List<string> { "ดนตรี", "ผ่อนคลาย", "กลางแจ้ง" },
                MaxParticipants = 2, CurrentParticipants = 2, MaxWaiting = 1, CurrentWaiting = 1, UserHostId = 101, IsRegistrationClosed = true,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 1, EventId = 1, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-2) },
                    new EventParticipation { Id = 2, EventId = 1, UserId = 105, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) },
                    new EventParticipation { Id = 3, EventId = 1, UserId = 104, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now }
                }
            },
            new Event
            {
                Id = 2, Title = "Workshop ทำเซรามิก", Description = "เรียนรูปั้นถ้วยกาแฟด้วยตัวเอง", Image = "https://img5.pic.in.th/file/secure-sv1/images204a713eaf5498ef.jpg", Location = "Thonglor Art Space", DateTime = DateTime.Now.AddDays(10).Date.AddHours(10), EndDateTime = DateTime.Now.AddDays(11).Date.AddHours(16), Tags = new List<string> { "Workshop", "ศิลปะ", "งานฝีมือ" },
                MaxParticipants = 10, CurrentParticipants = 8, MaxWaiting = 5, CurrentWaiting = 5, UserHostId = 102, IsRegistrationClosed = false,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 4, EventId = 2, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-10) },
                    new EventParticipation { Id = 5, EventId = 2, UserId = 107, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-9) },
                    new EventParticipation { Id = 6, EventId = 2, UserId = 108, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-8) },
                    new EventParticipation { Id = 7, EventId = 2, UserId = 109, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-7) },
                    new EventParticipation { Id = 8, EventId = 2, UserId = 110, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-6) },
                    new EventParticipation { Id = 9, EventId = 2, UserId = 111, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-5) },
                    new EventParticipation { Id = 10, EventId = 2, UserId = 112, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-4) },
                    new EventParticipation { Id = 11, EventId = 2, UserId = 113, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-3) },
                    new EventParticipation { Id = 12, EventId = 2, UserId = 104, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-2).AddHours(1) },
                    new EventParticipation { Id = 13, EventId = 2, UserId = 105, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-2).AddHours(2) },
                    new EventParticipation { Id = 14, EventId = 2, UserId = 106, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-2).AddHours(3) },
                    new EventParticipation { Id = 15, EventId = 2, UserId = 114, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-1).AddHours(1) },
                    new EventParticipation { Id = 16, EventId = 2, UserId = 115, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-1).AddHours(2) }
                }
            },
            new Event
            {
                Id = 5, Title = "Morning Run & Coffee", Description = "วิ่งเบา ๆ 5K แล้วไปกินกาแฟด้วยกัน", Image = "https://images.unsplash.com/photo-1520975661595-6453be3f7070", Location = "สวนเบญจกิติ", DateTime = DateTime.Now.AddDays(1).Date.AddHours(6), EndDateTime = DateTime.Now.AddDays(1).Date.AddHours(9), Tags = new List<string> { "กีฬา", "สุขภาพ", "coffee" }, MaxParticipants = 6, CurrentParticipants = 0, MaxWaiting = 3, CurrentWaiting = 0, UserHostId = 105, IsRegistrationClosed = false, Participants = new List<EventParticipation>()
            },
            new Event
            {
                Id = 6, Title = "Night Photography Walk", Description = "เดินถ่ายรูปกลางคืน", Image = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee", Location = "Bangkok Old Town", DateTime = DateTime.Now.AddDays(7).Date.AddHours(20), EndDateTime = DateTime.Now.AddDays(7).Date.AddHours(23), Tags = new List<string> { "ถ่ายรูป", "กลางคืน" }, MaxParticipants = 5, CurrentParticipants = 1, MaxWaiting = 2, CurrentWaiting = 0, UserHostId = 105, IsRegistrationClosed = false,
                Participants = new List<EventParticipation> { new EventParticipation { Id = 20, EventId = 6, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) } }
            },
            new Event
            {
                Id = 3, Title = "Board Game Night ลากยาว", Description = "เล่นบอร์ดเกมลากยาวข้ามคืนสำหรับสายฮาร์ดคอร์", Image = "https://images.unsplash.com/photo-1606509036992-4399d5c5afe4", Location = "Samyan Mitrtown", DateTime = DateTime.Now.AddDays(5).Date.AddHours(20), EndDateTime = DateTime.Now.AddDays(6).Date.AddHours(02), Tags = new List<string> { "เกม", "สังคม", "บอร์ดเกม" }, MaxParticipants = 5, CurrentParticipants = 0, MaxWaiting = 3, CurrentWaiting = 0, UserHostId = 103, IsRegistrationClosed = false, Participants = new List<EventParticipation>()
            },
            new Event
            {
                Id = 4, Title = "Street Food Tour เยาวราช", Description = "พาเดินกินของอร่อยยามค่ำคืน", Image = "https://images.unsplash.com/photo-1550547660-d9450f859349", Location = "Yaowarat Road", DateTime = DateTime.Now.AddDays(7).Date.AddHours(18), EndDateTime = DateTime.Now.AddDays(7).Date.AddHours(22), Tags = new List<string> { "อาหาร", "เที่ยวกลางคืน", "streetfood" }, MaxParticipants = 3, CurrentParticipants = 0, MaxWaiting = 2, CurrentWaiting = 0, UserHostId = 104, IsRegistrationClosed = false, Participants = new List<EventParticipation>()
            },
            new Event
            {
                Id = 9, Title = "Night Market Walk", Description = "เดินเที่ยวตลาดกลางคืน", Image = "https://images.unsplash.com/photo-1504674900247-0877df9cc836", Location = "Train Night Market", DateTime = DateTime.Now.AddDays(7).Date.AddHours(20), EndDateTime = DateTime.Now.AddDays(7).Date.AddHours(23), Tags = new List<string> { "ตลาด", "อาหาร", "nightlife" }, MaxParticipants = 5, CurrentParticipants = 1, MaxWaiting = 2, CurrentWaiting = 0, UserHostId = 105, IsRegistrationClosed = false,
                Participants = new List<EventParticipation> { new EventParticipation { Id = 30, EventId = 9, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) } }
            },
            
            // 🏁 🚨 Event 10 (ตัวจริง 3, สำรอง 2) ปิดรับสมัครแล้ว 
            new Event
            {
                Id = 10, Title = "Past Event: นิทรรศการศิลปะดิจิทัล", Description = "เดินชมนิทรรศการศิลปะที่จบไปแล้ว (ตัวจริง 3 สำรอง 2)", Image = "https://images.unsplash.com/photo-1536924940846-227afb31e2a5", Location = "BACC", DateTime = DateTime.Now.AddDays(-5).Date.AddHours(10), EndDateTime = DateTime.Now.AddDays(-5).Date.AddHours(18), Tags = new List<string> { "ศิลปะ", "นิทรรศการ", "จบแล้ว" },
                MaxParticipants = 10, CurrentParticipants = 3, MaxWaiting = 5, CurrentWaiting = 2, UserHostId = 101, IsRegistrationClosed = true,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 40, EventId = 10, UserId = 102, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-10) },
                    new EventParticipation { Id = 41, EventId = 10, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-9) },
                    new EventParticipation { Id = 42, EventId = 10, UserId = 104, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-8) },
                    new EventParticipation { Id = 43, EventId = 10, UserId = 108, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-7) },
                    new EventParticipation { Id = 44, EventId = 10, UserId = 109, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-6) }
                }
            },

            // 🏁 🚨 Event 11 (กำลังจัดอยู่ ตัวจริง 3, สำรอง 2) ปิดรับสมัครแล้ว 
            new Event
            {
                Id = 11, Title = "Ongoing Event: ล้อมวงเล่าเรื่องสยองขวัญ", Description = "กิจกรรมกำลังดำเนินอยู่! เข้ามาร่วมฟังเรื่องหลอนๆ สดๆ ด้วยกัน", Image = "https://images.unsplash.com/photo-1517604931442-7e0c8ed2963c", Location = "Discord Channel (Online)", DateTime = DateTime.Now.AddHours(-1), EndDateTime = DateTime.Now.AddHours(2), Tags = new List<string> { "ออนไลน์", "เล่าเรื่อง", "Ongoing" },
                MaxParticipants = 5, CurrentParticipants = 3, MaxWaiting = 5, CurrentWaiting = 2, UserHostId = 104, IsRegistrationClosed = true,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 50, EventId = 11, UserId = 102, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-3) },
                    new EventParticipation { Id = 51, EventId = 11, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-2) },
                    new EventParticipation { Id = 52, EventId = 11, UserId = 105, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) },
                    new EventParticipation { Id = 53, EventId = 11, UserId = 106, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-10) },
                    new EventParticipation { Id = 54, EventId = 11, UserId = 107, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-5) }
                }
            },
            
            // 🏁 🚨 Event 12 (งานใหญ่ที่คุณเป็น Host ตัวจริง 5, สำรอง 4) ปิดรับสมัครแล้ว
            new Event
            {
                Id = 12, Title = "Host Event: งานสัมมนา AI ยุคใหม่", Description = "งานสัมมนาที่คุณเป็นคนจัดและจบไปแล้ว", Image = "https://images.unsplash.com/photo-1485827404703-89b55fcc595e", Location = "BITEC Bangna", DateTime = DateTime.Now.AddDays(-10).Date.AddHours(9), EndDateTime = DateTime.Now.AddDays(-10).Date.AddHours(16), Tags = new List<string> { "สัมมนา", "AI", "Technology" },
                MaxParticipants = 5, CurrentParticipants = 5, MaxWaiting = 5, CurrentWaiting = 4, UserHostId = 102, IsRegistrationClosed = true,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 60, EventId = 12, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-12) },
                    new EventParticipation { Id = 61, EventId = 12, UserId = 105, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-11) },
                    new EventParticipation { Id = 62, EventId = 12, UserId = 106, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-11) },
                    new EventParticipation { Id = 63, EventId = 12, UserId = 107, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-11) },
                    new EventParticipation { Id = 64, EventId = 12, UserId = 108, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-11) },
                    new EventParticipation { Id = 65, EventId = 12, UserId = 109, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-10) },
                    new EventParticipation { Id = 66, EventId = 12, UserId = 110, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-9) },
                    new EventParticipation { Id = 67, EventId = 12, UserId = 111, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-8) },
                    new EventParticipation { Id = 68, EventId = 12, UserId = 112, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-7) }
                }
            },

            // 🏁 🚨 Event 13 (เคสทดสอบพิเศษ: งานจบไปแล้ว และล็อกอินปัจจุบันเป็นแค่ตัวสำรอง!)
            new Event
            {
                Id = 13, Title = "Past Event: ดูหนังมาราธอน (ตัวสำรอง)", Description = "งานจบไปแล้วและคุณเป็นแค่ตัวสำรอง เพื่อเทสต์หน้า My Events!", Image = "https://images.unsplash.com/photo-1489599849927-2ee91cede3ba", Location = "Major Cineplex", DateTime = DateTime.Now.AddDays(-2).Date.AddHours(13), EndDateTime = DateTime.Now.AddDays(-2).Date.AddHours(18), Tags = new List<string> { "หนัง", "พักผ่อน", "จบแล้ว" },
                MaxParticipants = 2, CurrentParticipants = 2, MaxWaiting = 5, CurrentWaiting = 2, UserHostId = 106, IsRegistrationClosed = true,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 70, EventId = 13, UserId = 104, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-10) },
                    new EventParticipation { Id = 71, EventId = 13, UserId = 105, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-9) },
                    new EventParticipation { Id = 72, EventId = 13, UserId = 102, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-8) }, // 🚨 User 102 (คนล็อกอิน) เป็นสำรอง
                    new EventParticipation { Id = 73, EventId = 13, UserId = 103, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-8).AddHours(2) } // 🚨 User 103 ก็เป็นสำรอง
                }
            }
        };

        // 2. จำลองข้อมูล User 
        public static List<User> UsersList = new List<User>()
        {
            new User { Id = 101, Username = "music_host", Password = "123", FName = "ก้องเกียรติ", SName = "ใจดี", Email = "kong@test.com", Gender = Genders.Male, Birthday = new DateTime(1990, 5, 20), Image = "https://ui-avatars.com/api/?name=Kong+J&background=random" },
            new User
            {
                Id = 102, Username = "art_host", Password = "123", FName = "ปั้นจั่น", SName = "งานละเอียด", Email = "pun@test.com", Gender = Genders.Female, Birthday = new DateTime(1995, 8, 15), Image = "https://ui-avatars.com/api/?name=Pun+N&background=random",
                Reviewslist = new List<Review>
                {
                    new Review { Id = 1, stars = 4, reviewtitle = "โฮสต์ดูแลดีมาก", reviewbody = "กิจกรรมสนุกมากครับ", UserId = 108, EventId = 12 },
                    new Review { Id = 2, stars = 4, reviewtitle = "แนะนำเลย", reviewbody = "เนื้อหาแน่นปึ๊ก", UserId = 106, EventId = 12 },
                    new Review { Id = 3, stars = 3, reviewtitle = "พอใช้ได้", reviewbody = "กิจกรรมน่าสนใจครับ", UserId = 103, EventId = 12 }
                }
            },
            
            // 👤 User 103 สมชาย:
            new User {
                Id = 103, Username = "somchai", Password = "123", FName = "สมชาย", SName = "เข็มกลัด", Email = "somchai@test.com", Gender = Genders.Male, Birthday = new DateTime(1985, 1, 1), Image = "https://ui-avatars.com/api/?name=Somchai+K&background=0D8ABC&color=fff",
                Settings  = new UserSettings { PrivateAccount = false, ShowEmail = true, ShowHostedEvents = false, ShowJoinedEvents = true },
                MyEvents = new List<EventParticipation> { new EventParticipation { Id = 1, EventId = 1, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-2) }, new EventParticipation { Id = 4, EventId = 2, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-10) } },
                Reviewslist = new List<Review>
                {
                    new Review { Id = 1, stars = 5, reviewtitle = "นิสัยดีมาก", reviewbody = "เป็นผู้เข้าร่วมที่ตรงต่อเวลา คุยสนุกครับ", UserId = 101, EventId = 10 },
                    new Review { Id = 2, stars = 4, reviewtitle = "โอเคเลย", reviewbody = "เป็นกันเองดีครับ", UserId = 102, EventId = 12 }
                }
            },

            // 👤 User 104 แนนซี่:
            new User {
                Id = 104, Username = "nancy", Password = "123", FName = "แนนซี่", SName = "มีตังค์", Email = "nancy@test.com", Gender = Genders.Female, Birthday = new DateTime(2000, 12, 25), Image = "https://ui-avatars.com/api/?name=Nancy+M&background=FFC107",
                Settings = new UserSettings { PrivateAccount = true, ShowEmail = false, ShowHostedEvents = true, ShowJoinedEvents = true },
                Reviewslist = new List<Review>
                {
                    new Review { Id = 1, stars = 5, reviewtitle = "เยี่ยมมาก", reviewbody = "น่ารัก ช่วยเหลืองานดีมาก", UserId = 101, EventId = 10 }
                }
            },

            // User อื่นๆ ที่ยังไม่มีรีวิว
            new User { Id = 105, Username = "piti", Password = "123", FName = "ปิติ", SName = "พอใจ", Email = "piti@test.com", Gender = Genders.Male, Birthday = new DateTime(1998, 3, 10), Image = "https://ui-avatars.com/api/?name=Piti+P&background=8E44AD&color=fff", Settings = new UserSettings { PrivateAccount = false, ShowEmail = true, ShowHostedEvents = true, ShowJoinedEvents = false } },
            new User { Id = 106, Username = "chujai", Password = "123", FName = "ชูใจ", SName = "เลิศล้ำ", Email = "chujai@test.com", Gender = Genders.Other, Birthday = new DateTime(1992, 11, 5), Image = "https://ui-avatars.com/api/?name=Chujai+L&background=E74C3C&color=fff" },
            new User { Id = 107, Username = "manee", Password = "123", FName = "มานี", SName = "รักดี", Email = "manee@test.com", Gender = Genders.Female, Birthday = new DateTime(1996, 2, 14), Image = "https://ui-avatars.com/api/?name=Manee+R&background=random", Reviewslist = new List<Review> { new Review { Id = 1, stars = 3, reviewtitle = "กลางๆ", reviewbody = "มาสายไปนิดนึงครับ", UserId = 102, EventId = 12 } } },
            new User { Id = 108, Username = "veera", Password = "123", FName = "วีระ", SName = "กล้าหาญ", Email = "veera@test.com", Gender = Genders.Male, Birthday = new DateTime(1991, 7, 20), Image = "https://ui-avatars.com/api/?name=Veera+K&background=random" },
            new User { Id = 109, Username = "arthit", Password = "123", FName = "อาทิตย์", SName = "สว่าง", Email = "arthit@test.com", Gender = Genders.Male, Birthday = new DateTime(1994, 9, 9), Image = "https://ui-avatars.com/api/?name=Arthit+S&background=random" },
            new User { Id = 110, Username = "junpen", Password = "123", FName = "จันทร์เพ็ญ", SName = "งามตา", Email = "junpen@test.com", Gender = Genders.Female, Birthday = new DateTime(1997, 10, 31), Image = "https://ui-avatars.com/api/?name=Junpen+N&background=random" },
            new User { Id = 111, Username = "somying", Password = "123", FName = "สมหญิง", SName = "จริงใจ", Email = "somying@test.com", Gender = Genders.Female, Birthday = new DateTime(1989, 4, 12), Image = "https://ui-avatars.com/api/?name=Somying+J&background=random" },
            new User { Id = 112, Username = "thana", Password = "123", FName = "ธนา", SName = "พาณิชย์", Email = "thana@test.com", Gender = Genders.Male, Birthday = new DateTime(1993, 6, 6), Image = "https://ui-avatars.com/api/?name=Thana+P&background=random" },
            new User { Id = 113, Username = "wipa", Password = "123", FName = "วิภา", SName = "วาที", Email = "wipa@test.com", Gender = Genders.Female, Birthday = new DateTime(1999, 1, 1), Image = "https://ui-avatars.com/api/?name=Wipa+W&background=random" },
            new User { Id = 114, Username = "kritsana", Password = "123", FName = "กฤษณะ", SName = "สีนวล", Email = "kritsana@test.com", Gender = Genders.Male, Birthday = new DateTime(1988, 8, 8), Image = "https://ui-avatars.com/api/?name=Kritsana+S&background=random" },
            new User { Id = 115, Username = "rattana", Password = "123", FName = "รัตนา", SName = "พรหมมินทร์", Email = "rattana@test.com", Gender = Genders.Female, Birthday = new DateTime(1995, 3, 3), Image = "https://ui-avatars.com/api/?name=Rattana+P&background=random" },
            new User { Id = 116, Username = "yodying", Password = "123", FName = "ยอดยิ่ง", SName = "ชิงชัย", Email = "yodying@test.com", Gender = Genders.Male, Birthday = new DateTime(1992, 4, 15), Image = "https://ui-avatars.com/api/?name=Yodying+C&background=random" }
        };
    }
}