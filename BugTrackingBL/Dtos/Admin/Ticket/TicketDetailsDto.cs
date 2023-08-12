namespace BugTrackingBL.Dtos.Admin
{
    public class TicketDetailsDto
    {
        public required string TicketTitle { get; set; } = string.Empty;
        public required string TicketDescription { get; set; } = string.Empty;
    }
}
