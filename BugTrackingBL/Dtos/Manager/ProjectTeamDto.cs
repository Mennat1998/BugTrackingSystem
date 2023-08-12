namespace BugTrackingBL.Dtos.Manager
{
    public class ProjectTeamDto
    {
        public int ProjectId { get; set; }
        public string TesterId { get; set; } = string.Empty;

        public List<string> DevelopersIds { get; set; }= new List<string>();
    }
}
