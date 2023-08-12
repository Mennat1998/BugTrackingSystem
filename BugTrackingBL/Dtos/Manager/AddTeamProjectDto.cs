namespace BugTrackingBL.Dtos.Manager
{
    public class AddTeamProjectDto
    {
        public int projectid { get; set; }
        public string testerId { get; set; } = string.Empty;

        public List<string> DevelopersIds = new List<string>();
    }
}
