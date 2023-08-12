
namespace BugTrackingDAL.Repos
{
    public interface IManagerRepo
    {
        List<Project> GetAllProjects(string Manager_Id);
        Project? GetProject(int ProjectId);
        IEnumerable<Ticket> DetailsOfTicket(int projectid);
        Ticket? DetailsOfTicketwithattachment(int ticketid);
        void AddProjectTeam(Project project);
        void UpdateProjectTeam(Project project);
        void AssignTicket(Ticket ticket);
        Developer? GetDeveloper(string id);
        Ticket? GetTicket(int id);
        List<Developer> GetAllDevelopersNames();

        List<Ticket> GetAllTickets(string managerid);
        Tester? GetTester(string Testerid);
        void AddComment(Comments comment);

        int SaveChanges();
    }
}
