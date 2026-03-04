namespace WebApplicationProject.Models
{
    public class ProfilePageViewModel
    {
        public User UserInfo { get; set; }
        public List<Event> HostedEvents { get; set; }
        public List<Event> JoinedEvents { get; set; }
    }
}
