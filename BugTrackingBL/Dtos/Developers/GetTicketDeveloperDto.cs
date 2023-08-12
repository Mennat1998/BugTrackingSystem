using BugTrackingBL.Dtos.Testers;

namespace BugTrackingBL.Dtos.Developers
{
    public enum TicketStatus
    {
        Completed,
        InProgress,
        Resolved,

    }
    public class GetTicketDeveloperDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public List<AttachmentDto> Attachments { get; set; }= new List<AttachmentDto> { };
        
        
    }
}
