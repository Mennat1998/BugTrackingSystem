namespace BugTrackingBL.Dtos.Testers
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
    }
}
