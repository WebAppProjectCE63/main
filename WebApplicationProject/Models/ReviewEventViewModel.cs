namespace WebApplicationProject.Models
{
    public class ReviewEventViewModel
    {
        public Event EventData { get; set; }
        public int CurrentLoggedInUserId { get; set; }
        public List<ReviewTargetUserModel> TargetUsers { get; set; } = new List<ReviewTargetUserModel>();
    }

    public class ReviewTargetUserModel
    {
        public User UserInfo { get; set; }
        public bool IsHost { get; set; }
        public DateTime JoinedAt { get; set; }
        public Review MyReviewToThisUser { get; set; }
    }
}
