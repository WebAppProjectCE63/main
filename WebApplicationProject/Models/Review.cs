namespace WebApplicationProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int stars { get; set; }
        public string reviewtitle { get; set; }
        public string reviewbody { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
