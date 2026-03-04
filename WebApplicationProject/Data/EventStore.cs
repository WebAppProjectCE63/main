using System;
using System.Collections.Generic;
using WebApplicationProject.Models;

namespace WebApplicationProject.Data
{
    public static class EventStore
    {
        public static List<Event> Events = new List<Event>
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
                    new EventParticipation
                    {
                        Id = 1,
                        EventId = 1,
                        UserId = 103,
                        Status = ParticipationStatus.Confirmed,
                        JoinedAt = DateTime.Now.AddDays(-2)
                    },
                    new EventParticipation
                    {
                        Id = 2,
                        EventId = 1,
                        UserId = 105,
                        Status = ParticipationStatus.Confirmed,
                        JoinedAt = DateTime.Now.AddDays(-1)
                    },
                    new EventParticipation
                    {
                        Id = 3,
                        EventId = 1,
                        UserId = 104,
                        Status = ParticipationStatus.Waiting,
                        JoinedAt = DateTime.Now
                    }
                }
            },

            // ----------------------------------------------------
            // 🎨 Event 2: Workshop เซรามิก
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
                CurrentParticipants = 1,
                UserHostId = 102,

                Participants = new List<EventParticipation>
                {
                    new EventParticipation
                    {
                        Id = 4,
                        EventId = 2,
                        UserId = 103,
                        Status = ParticipationStatus.Confirmed,
                        JoinedAt = DateTime.Now.AddDays(-5)
                    },
                    new EventParticipation
                    {
                        Id = 5,
                        EventId = 2,
                        UserId = 106,
                        Status = ParticipationStatus.Waiting,
                        JoinedAt = DateTime.Now.AddDays(-1)
                    }
                }
            }
        };
    }
}