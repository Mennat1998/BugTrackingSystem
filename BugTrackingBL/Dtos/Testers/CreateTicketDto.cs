namespace BugTrackingBL.Dtos.Testers
{
        public enum TicketStatus
        {
            Completed,
            InProgress,
            Resolved,

        }
    public class CreateTicketDto
    {           
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            //public IEnumerable<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
            public int ProjectId { get; set; }
    }
}
