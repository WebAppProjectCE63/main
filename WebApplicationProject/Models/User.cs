namespace WebApplicationProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public List<EventParticipation> MyEvents { get; set; } = new List<EventParticipation>();
        public List<Review> Reviewslist { get; set; } = new List<Review>();
        public UserSettings Settings { get; set; } = new UserSettings();
        
    }
    public class UserSettings
    {
        public bool PrivateAccount { get; set; } = false;
        public bool ShowEmail { get; set; } = true;
        public bool ShowHostedEvents { get; set; } = true;
        public bool ShowJoinedEvents { get; set; } = true;
    }
}
 