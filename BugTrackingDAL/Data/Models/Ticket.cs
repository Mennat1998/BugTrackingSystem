using System.ComponentModel.DataAnnotations.Schema;
namespace BugTrackingDAL
{
    public enum TicketStatus
    {
        Completed,
        InProgress,
        Resolved,
    }
    public class Ticket 
    {
        public int TicketId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        [ForeignKey("Tester")]
        public string? TesterId { get; set; }
        public Tester? Tester { get; set; }  // Navigation property

        [ForeignKey("Manager")]
        public string? ManagerId { get; set; }
        public Manager? Manager { get; set; }  // Navigation property

        [ForeignKey("Developer")]
        public string? DeveloperId { get; set; }
        public Developer? Developer { get; set; }  // Navigation property

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }  // Navigation property
        public ICollection<Attachments> Attachments { get; set; }= new HashSet<Attachments>();
        public ICollection<Comments> Comments { get; set; } = new HashSet<Comments>();

    }
}
