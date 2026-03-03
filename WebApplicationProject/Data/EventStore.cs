using System;
using System.Collections.Generic;
using WebApplicationProject.Models;

namespace WebApplicationProject.Data
{
    public static class EventStore
    {
        public static List<Event> Events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Music Festival",
                Description = "Outdoor music festival with live bands.",
                Image = "",
                Location = "Bangkok",
                DateTime = DateTime.Now.AddDays(5),
                MaxParticipants = 100,
                CurrentParticipants = 45,
                UserHostId = 1
            },
            new Event
            {
                Id = 2,
                Title = "Tech Meetup",
                Description = "Discuss latest trends in software development.",
                Image = "",
                Location = "Chiang Mai",
                DateTime = DateTime.Now.AddDays(10),
                MaxParticipants = 50,
                CurrentParticipants = 20,
                UserHostId = 2
            }
        };
    }
}