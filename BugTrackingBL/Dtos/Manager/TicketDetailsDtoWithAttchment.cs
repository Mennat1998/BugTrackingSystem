namespace BugTrackingBL.Dtos.Manager
{
    public class TicketDetailsDtoWithAttchment
    {
        public required string TicketTitle { get; set; } = string.Empty;
        public required string TicketDescription { get; set; } = string.Empty;

        public List<AttachmentDto> AttachmentDetails { get; set; } = new List<AttachmentDto>();
    }
}
