namespace BugTrackingBL
{
    public class ProjectDetailsReadDto
    {
        public int ProId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ManagerName { get; set; } = string.Empty;
        public string? TesterName { get; set; } = string.Empty;
        public List<string>? DevelopersNames { get; set; }
        public int TicketsNo { get; set; } 
    }
}
