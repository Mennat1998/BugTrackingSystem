using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackingDAL
{
    public class Attachments
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        [ForeignKey("Ticket")]
        public int TickectId { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
