using System.Collections.Generic;

namespace WebApplicationProject.Models
{
    public class ProfilePageViewModel
    {
        public User UserInfo { get; set; }
        public int CurrentLoggedInUserId { get; set; }
        public List<EventDisplayModel> HostedEvents { get; set; } = new List<EventDisplayModel>();
        public List<EventDisplayModel> JoinedEvents { get; set; } = new List<EventDisplayModel>();
        public List<ReviewDisplayModel> Reviews { get; set; } = new List<ReviewDisplayModel>();
    }
    public class EventDisplayModel
    {
        public Event EventData { get; set; }
        public List<User> ParticipantAvatars { get; set; } = new List<User>();
    }
    public class ReviewDisplayModel
    {
        public Review ReviewData { get; set; }
        public User Author { get; set; }
        public string EventTitle { get; set; }
    }
}