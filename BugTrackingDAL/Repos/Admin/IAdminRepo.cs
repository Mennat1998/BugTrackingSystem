namespace BugTrackingDAL
{
    public interface IAdminRepo 
    {
        IEnumerable<Project> GetAllProjects();
        Project? GetProjectById(int id);
        void AddProject (Project project);
        IEnumerable<GeneralUser> GetAllEmployees();
        GeneralUser? GetEmployeeById(string id);
        void  AddEmployee(GeneralUser employee);
        void UpgradeEmployeeRole(string employeeId , string newRole);
        void RemoveEmployee(string employeeId);
        IEnumerable<Ticket> DetailsOfTicket(int projectid);
        Ticket? DetailsOfTicketwithattachment(int ticketid);
        int SaveChanges(); 
    }
}
