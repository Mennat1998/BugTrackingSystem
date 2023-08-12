namespace BugTrackingBL.Dtos.Developers
{
    
    public class HandleTicketDto
    {
        public int Id { get; set; }
        public TicketStatus Status { get; set; }
        public string URL { get; set; } = string.Empty;
    }
}
