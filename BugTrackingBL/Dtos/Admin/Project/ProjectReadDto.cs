namespace BugTrackingBL
{
    public class ProjectReadDto
    {
        public int ProId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DevelopersNo { get; set; } 
        public int TicketsNo { get; set; }
    }
}
