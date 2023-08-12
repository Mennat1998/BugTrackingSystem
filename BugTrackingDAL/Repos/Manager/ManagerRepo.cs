using Microsoft.EntityFrameworkCore;

namespace BugTrackingDAL.Repos
{
    public class ManagerRepo : IManagerRepo
    {
        private readonly BugContext _BugContext;

        public ManagerRepo(BugContext BugContext)
        {
            _BugContext = BugContext;
        }
        public List<Project> GetAllProjects(string Manager_Id)
        {

            return _BugContext.Projects
                .Where(a => a.MangerId == Manager_Id).ToList();
        }
        public Project? GetProject(int ProjectId)
        {

            return _BugContext.Projects
                .Include(D => D.Developers)
                .FirstOrDefault(a => a.ProjectId == ProjectId);
               
        }
        public IEnumerable<Ticket> DetailsOfTicket(int projectid)
        {
            return _BugContext.Tickets.Where(a => a.ProjectId == projectid);

        }

        public Ticket? DetailsOfTicketwithattachment(int ticketid)
        {
            return _BugContext.Set<Ticket>()
                .Include(a => a.Attachments)
                .FirstOrDefault(a => a.TicketId == ticketid);

        }
        public Ticket? GetTicket(int id)
        {
            return _BugContext.Tickets.FirstOrDefault(a => a.TicketId == id);
        }
        public void AddProjectTeam(Project project)
        {
            // No Implementation as we as using Tracking 
           // _BugContext.Projects.Add(proj);
        }
        public void UpdateProjectTeam(Project project)
        {
            // No Implementation as we as using Tracking 
        }

        public void AssignTicket(Ticket ticket)
        {
            _BugContext.Update(ticket);
        }
        public Developer? GetDeveloper(string id)
        {
            return _BugContext.Developers.FirstOrDefault( a=>a.Id==id && a.Type=="Developer");

        }
        public void AddComment(Comments comment)
        {
            _BugContext.Comments.Add(comment);
        }

        public Tester? GetTester(string Testerid)
        {
            return _BugContext.Testers.FirstOrDefault(a => a.Id ==Testerid );
        }

        public List<Developer> GetAllDevelopersNames()
        {
            return _BugContext.Developers.ToList();
        }

        public List<Ticket> GetAllTickets(string managerid)
        {
            return _BugContext.Tickets.Where(T => T.ManagerId == managerid).ToList();
        }
        public int SaveChanges()
        {
            return _BugContext.SaveChanges();
        }

    }
}
