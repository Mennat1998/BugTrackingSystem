namespace BugTrackingBL.Dtos.Manager
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public string TesterName { get; set; } = string.Empty;
        public List<string> Developers { get; set; } = new List<string>();
    }
}
