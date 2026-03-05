namespace WebApplicationProject.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public int UserHostId { get; set; }
        public List<EventParticipation> Participants { get; set; } = new List<EventParticipation>();
    }
}
