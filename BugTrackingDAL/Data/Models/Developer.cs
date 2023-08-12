namespace BugTrackingDAL
{
    public class Developer : GeneralUser
    {
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();
    }
}
