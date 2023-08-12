using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackingDAL
{
    public class Project 
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ForeignKey("Manager")]
        public string? MangerId { get; set; }
        public Manager? Manager { get; set; }

        [ForeignKey("Tester")]
        public string? TesterId { get; set; }
        public Tester? Tester { get; set; }
        public  ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public ICollection<Developer> Developers { get; set; } = new HashSet<Developer>();
    }
}
