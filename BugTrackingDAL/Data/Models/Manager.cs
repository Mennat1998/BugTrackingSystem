namespace BugTrackingDAL
{
    public class Manager : GeneralUser
    {
      public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
      public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
    }
}
