namespace BugTrackingBL.Dtos.Admin
{
    public class TicketDetailsDtoWithAttchment
    {
        public required string TicketTitle { get; set; } = string.Empty;
        public required string TicketDescription { get; set; } = string.Empty;

        public List<AttachmentDto> AttachmentDetails { get; set; } = new List<AttachmentDto>();
        public List<CommentReadDto> CommentsDetails { get; set; } = new List<CommentReadDto>();


    }
}
