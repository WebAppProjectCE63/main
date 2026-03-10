using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int stars { get; set; }
        public string reviewtitle { get; set; }
        public string reviewbody { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User Writer { get; set; }

        public int TargetUserId { get; set; }
        [ForeignKey("TargetUserId")]
        public virtual User TargetUser { get; set; }

        public int EventId { get; set; }
        public bool IsAnonymous { get; set; } = false;
    }
}
