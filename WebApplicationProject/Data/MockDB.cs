using WebApplicationProject.Models;

namespace WebApplicationProject.Data
{
    public class MockDB
    {
        public static int CurrentLoggedInUserId = 102;
        // 1. จำลองข้อมูล Event (ที่มีรายชื่อคนอยู่ข้างในแล้ว)
        public static List<Event> EventList = new List<Event>()
        {
            // ----------------------------------------------------
            // 🎵 Event 1: ดนตรีในสวน
            // ----------------------------------------------------
            new Event
            {
                Id = 1,
                Title = "ดนตรีในสวน (Music in the Park)",
                Description = "มาร่วมฟังดนตรีสดบรรยากาศชิลๆ ยามเย็น",
                Image = "https://img2.pic.in.th/cover-1.md.jpg",
                Location = "สวนลุมพินี กรุงเทพฯ",
                DateTime = DateTime.Now.AddDays(3),
                Tags = new List<string> { "ดนตรี", "ผ่อนคลาย", "กลางแจ้ง" },
                MaxParticipants = 2,
                CurrentParticipants = 2,
                UserHostId = 101,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 1, EventId = 1, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-2) },
                    new EventParticipation { Id = 2, EventId = 1, UserId = 105, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) },
                    new EventParticipation { Id = 3, EventId = 1, UserId = 104, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now }
                }
            },

            // ----------------------------------------------------
            // 🎨 Event 2: Workshop เซรามิก (🌟 อัปเดตข้อมูลสำหรับเทสต์ระบบ Manage)
            // ----------------------------------------------------
            new Event
            {
                Id = 2,
                Title = "Workshop ทำเซรามิก",
                Description = "เรียนรูปั้นถ้วยกาแฟด้วยตัวเอง",
                Image = "https://img5.pic.in.th/file/secure-sv1/images204a713eaf5498ef.jpg",
                Location = "Thonglor Art Space",
                DateTime = DateTime.Now.AddDays(10),
                Tags = new List<string> { "Workshop", "ศิลปะ", "งานฝีมือ" },
                MaxParticipants = 10,
                CurrentParticipants = 8, // ✅ ตั้งเป็น 8 คน (เหลือที่ว่าง 2 ที่)
                UserHostId = 102,
                Participants = new List<EventParticipation>
                {
                    // 🟢 โซนตัวจริง (Confirmed) 8 คน
                    new EventParticipation { Id = 4, EventId = 2, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-10) }, // สมชาย
                    new EventParticipation { Id = 6, EventId = 2, UserId = 107, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-9) }, // มานี
                    new EventParticipation { Id = 7, EventId = 2, UserId = 108, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-8) }, // วีระ
                    new EventParticipation { Id = 8, EventId = 2, UserId = 109, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-7) }, // อาทิตย์
                    new EventParticipation { Id = 9, EventId = 2, UserId = 110, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-6) }, // จันทร์เพ็ญ
                    new EventParticipation { Id = 10, EventId = 2, UserId = 111, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-5) },// สมหญิง
                    new EventParticipation { Id = 11, EventId = 2, UserId = 112, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-4) },// ธนา
                    new EventParticipation { Id = 12, EventId = 2, UserId = 113, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-3) },// วิภา

                    // 🟡 โซนตัวสำรอง (Waiting) 5 คน
                    new EventParticipation { Id = 5, EventId = 2, UserId = 106, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-2) }, // ชูใจ (คิว 1)
                    new EventParticipation { Id = 13, EventId = 2, UserId = 114, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddDays(-1) },// กฤษณะ (คิว 2)
                    new EventParticipation { Id = 14, EventId = 2, UserId = 115, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-20) },// รัตนา (คิว 3)
                    new EventParticipation { Id = 15, EventId = 2, UserId = 116, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-10) },// เดชา (คิว 4)
                    new EventParticipation { Id = 16, EventId = 2, UserId = 117, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-5) } // อารยา (คิว 5)
                }
            },
            new Event
            {
                Id = 5,
                Title = "Morning Run & Coffee",
                Description = "วิ่งเบา ๆ 5K แล้วไปกินกาแฟด้วยกัน",
                Image = "https://images.unsplash.com/photo-1520975661595-6453be3f7070",
                Location = "สวนเบญจกิติ",
                DateTime = DateTime.Now.AddDays(12),
                Tags = new List<string> { "กีฬา", "สุขภาพ", "coffee" },
                MaxParticipants = 6,
                CurrentParticipants = 6, // เต็มพอดี เพื่อทดสอบ Waiting
                UserHostId = 105,

                Participants = new List<EventParticipation>
                {
                    // 🟢 Confirmed 6 คน (เต็มแล้ว)
                    new EventParticipation { Id = 40, EventId = 5, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-6) },
                    new EventParticipation { Id = 41, EventId = 5, UserId = 104, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-5) },
                    new EventParticipation { Id = 42, EventId = 5, UserId = 106, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-4) },
                    new EventParticipation { Id = 43, EventId = 5, UserId = 107, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-3) },
                    new EventParticipation { Id = 44, EventId = 5, UserId = 108, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-2) },
                    new EventParticipation { Id = 45, EventId = 5, UserId = 109, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) },

                    // 🟡 Waiting 3 คน
                    new EventParticipation { Id = 46, EventId = 5, UserId = 110, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-20) },
                    new EventParticipation { Id = 47, EventId = 5, UserId = 111, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-12) },
                    new EventParticipation { Id = 48, EventId = 5, UserId = 112, Status = ParticipationStatus.Waiting, JoinedAt = DateTime.Now.AddHours(-6) }
                }
            },
             // ----------------------------------------------------
    // 🎮 Event 3: Board Game Night
    // ----------------------------------------------------
            new Event
            {
                Id = 3,
                Title = "Board Game Night",
                Description = "เล่นบอร์ดเกมกับเพื่อนใหม่หลังเลิกงาน",
                Image = "https://images.unsplash.com/photo-1606509036992-4399d5c5afe4",
                Location = "Samyan Mitrtown",
                DateTime = DateTime.Now.AddDays(5),
                Tags = new List<string> { "เกม", "สังคม", "บอร์ดเกม" },
                MaxParticipants = 5,
                CurrentParticipants = 2,
                UserHostId = 103,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 20, EventId = 3, UserId = 104, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) },
                    new EventParticipation { Id = 21, EventId = 3, UserId = 105, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddHours(-5) }
                }
            },

            // ----------------------------------------------------
            // 🍔 Event 4: Street Food Tour
            // ----------------------------------------------------
            new Event
            {
                Id = 4,
                Title = "Street Food Tour เยาวราช",
                Description = "พาเดินกินของอร่อยยามค่ำคืน",
                Image = "https://images.unsplash.com/photo-1550547660-d9450f859349",
                Location = "Yaowarat Road",
                DateTime = DateTime.Now.AddDays(7),
                Tags = new List<string> { "อาหาร", "เที่ยวกลางคืน", "streetfood" },
                MaxParticipants = 3,
                CurrentParticipants = 3,
                UserHostId = 104,
                Participants = new List<EventParticipation>
                {
                    new EventParticipation { Id = 30, EventId = 4, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-2) },
                    new EventParticipation { Id = 31, EventId = 4, UserId = 105, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-1) },
                    new EventParticipation { Id = 32, EventId = 4, UserId = 106, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddHours(-3) }
                }
            }
        };
 

        // ----------------------------------------------------

        // 2. จำลองข้อมูล User 
        public static List<User> UsersList = new List<User>()
        {
            new User { Id = 101, Username = "music_host", Password = "123", FName = "ก้องเกียรติ", SName = "ใจดี", Email = "kong@test.com", Gender = "Male", Birthday = new DateTime(1990, 5, 20), Image = "https://ui-avatars.com/api/?name=Kong+J&background=random" },
            new User
            {
                Id = 102, Username = "art_host", Password = "123", FName = "ปั้นจั่น", SName = "งานละเอียด", Email = "pun@test.com", Gender = "Female", Birthday = new DateTime(1995, 8, 15), Image = "https://ui-avatars.com/api/?name=Pun+N&background=random",
                Reviewslist = new List<Review>
                {
                    new Review
                    {
                        Id = 1,
                        stars = 5,
                        reviewtitle = "โฮสต์ดูแลดีมาก",
                        reviewbody = "กิจกรรมสนุกมากครับ โฮสต์เป็นกันเองสุดๆ",
                        UserId = 108,
                    },
                    new Review
                    {
                        Id = 2,
                        stars = 4,
                        reviewtitle = "แนะนำเลย",
                        reviewbody = "เนื้อหาแน่นปึ๊ก แต่สถานที่แอบแคบไปนิดนึง",
                        UserId = 106,
                    },
                    new Review
                    {
                        Id = 3,
                        stars = 3,
                        reviewtitle = "พอใช้ได้",
                        reviewbody = "กิจกรรมน่าสนใจครับ",
                        UserId = 103,
                    }
                }
            },

            // 👤 Users เดิม
            new User { Id = 103, Username = "somchai", Password = "123", FName = "สมชาย", SName = "เข็มกลัด", Email = "somchai@test.com", Gender = "Male", Birthday = new DateTime(1985, 1, 1), Image = "https://ui-avatars.com/api/?name=Somchai+K&background=0D8ABC&color=fff",
            Settings  = new UserSettings
            { 
                PrivateAccount = false,
                ShowEmail = true,
                ShowHostedEvents = false,
                ShowJoinedEvents = true
            },
                MyEvents = new List<EventParticipation>
                {
                    new EventParticipation { Id = 1, EventId = 1, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-2) },
                    new EventParticipation { Id = 4, EventId = 2, UserId = 103, Status = ParticipationStatus.Confirmed, JoinedAt = DateTime.Now.AddDays(-10) }
                }
            },
            new User { Id = 104, Username = "nancy", Password = "123", FName = "แนนซี่", SName = "มีตังค์", Email = "nancy@test.com", Gender = "Female", Birthday = new DateTime(2000, 12, 25), Image = "https://ui-avatars.com/api/?name=Nancy+M&background=FFC107",
            Settings = new UserSettings
            {
                PrivateAccount = true,
                ShowEmail = false,
                ShowHostedEvents = true,
                ShowJoinedEvents = true
            },},
            new User { Id = 105, Username = "piti", Password = "123", FName = "ปิติ", SName = "พอใจ", Email = "piti@test.com", Gender = "Male", Birthday = new DateTime(1998, 3, 10), Image = "https://ui-avatars.com/api/?name=Piti+P&background=8E44AD&color=fff",
            Settings = new UserSettings
            {
                PrivateAccount = false,
                ShowEmail = true,
                ShowHostedEvents = true,
                ShowJoinedEvents = false
            },},
            new User { Id = 106, Username = "chujai", Password = "123", FName = "ชูใจ", SName = "เลิศล้ำ", Email = "chujai@test.com", Gender = "LGBTQ+", Birthday = new DateTime(1992, 11, 5), Image = "https://ui-avatars.com/api/?name=Chujai+L&background=E74C3C&color=fff" },

            // 🌟 Users ใหม่
            new User { Id = 107, Username = "manee", Password = "123", FName = "มานี", SName = "รักดี", Email = "manee@test.com", Gender = "Female", Birthday = new DateTime(1996, 2, 14), Image = "https://ui-avatars.com/api/?name=Manee+R&background=random" },
            new User { Id = 108, Username = "veera", Password = "123", FName = "วีระ", SName = "กล้าหาญ", Email = "veera@test.com", Gender = "Male", Birthday = new DateTime(1991, 7, 20), Image = "https://ui-avatars.com/api/?name=Veera+K&background=random" },
            new User { Id = 109, Username = "arthit", Password = "123", FName = "อาทิตย์", SName = "สว่าง", Email = "arthit@test.com", Gender = "Male", Birthday = new DateTime(1994, 9, 9), Image = "https://ui-avatars.com/api/?name=Arthit+S&background=random" },
            new User { Id = 110, Username = "junpen", Password = "123", FName = "จันทร์เพ็ญ", SName = "งามตา", Email = "junpen@test.com", Gender = "Female", Birthday = new DateTime(1997, 10, 31), Image = "https://ui-avatars.com/api/?name=Junpen+N&background=random" },
            new User { Id = 111, Username = "somying", Password = "123", FName = "สมหญิง", SName = "จริงใจ", Email = "somying@test.com", Gender = "Female", Birthday = new DateTime(1989, 4, 12), Image = "https://ui-avatars.com/api/?name=Somying+J&background=random" },
            new User { Id = 112, Username = "thana", Password = "123", FName = "ธนา", SName = "พาณิชย์", Email = "thana@test.com", Gender = "Male", Birthday = new DateTime(1993, 6, 6), Image = "https://ui-avatars.com/api/?name=Thana+P&background=random" },
            new User { Id = 113, Username = "wipa", Password = "123", FName = "วิภา", SName = "วาที", Email = "wipa@test.com", Gender = "Female", Birthday = new DateTime(1999, 1, 1), Image = "https://ui-avatars.com/api/?name=Wipa+W&background=random" },
            new User { Id = 114, Username = "kritsana", Password = "123", FName = "กฤษณะ", SName = "สีนวล", Email = "kritsana@test.com", Gender = "Male", Birthday = new DateTime(1988, 8, 8), Image = "https://ui-avatars.com/api/?name=Kritsana+S&background=random" },
            new User { Id = 115, Username = "rattana", Password = "123", FName = "รัตนา", SName = "พรหมมินทร์", Email = "rattana@test.com", Gender = "Female", Birthday = new DateTime(1995, 3, 3), Image = "https://ui-avatars.com/api/?name=Rattana+P&background=random" },
            new User { Id = 116, Username = "decha", Password = "123", FName = "เดชา", SName = "ชาญวิทย์", Email = "decha@test.com", Gender = "Male", Birthday = new DateTime(1992, 12, 12), Image = "https://ui-avatars.com/api/?name=Decha+C&background=random" },
            new User { Id = 117, Username = "araya", Password = "123", FName = "อารยา", SName = "ศิริ", Email = "araya@test.com", Gender = "LGBTQ+", Birthday = new DateTime(2001, 5, 5), Image = "https://ui-avatars.com/api/?name=Araya+S&background=random" }
        };
    }
}