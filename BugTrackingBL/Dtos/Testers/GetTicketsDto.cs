namespace BugTrackingBL.Dtos.Testers
{
    public class GetTicketsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public ICollection<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
    }
}
