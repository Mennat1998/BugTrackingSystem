namespace BugTrackingBL.Dtos.Manager
{
    public class TicketDetailsDto
    {
        public int ProjectId { get; set; }
        public required string TicketTitle { get; set; } = string.Empty;
        public required string TicketDescription { get; set; } = string.Empty;
    }
}
