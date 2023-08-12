using Microsoft.EntityFrameworkCore;

namespace BugTrackingDAL
{
    public class AdminRepo : IAdminRepo
    {
        private readonly BugContext _context;
        public AdminRepo(BugContext context)
        {
            _context = context;
        }
        #region Project Crud
        IEnumerable<Project> IAdminRepo.GetAllProjects()
        {
            return _context.Projects
                .Include(a => a.Developers)
                .Include(t => t.Tickets);
        }
        public Project? GetProjectById(int id)
        {
            return _context.Projects
                .Include(a => a.Developers)
                .Include(t => t.Tickets)
                .FirstOrDefault(i=>i.ProjectId == id);
        }
        void IAdminRepo.AddProject(Project project)
        {
            _context.Projects.Add(project);
        }
        #endregion

        #region Employees Crud
        public IEnumerable<GeneralUser> GetAllEmployees()
        {
            return _context.Users.Where(a=>a.Type != "Unauthorized");
        }
        GeneralUser? IAdminRepo.GetEmployeeById(string id)
        {
            return _context.Users.FirstOrDefault(a=> a.Id == id);
        }
        public void AddEmployee(GeneralUser employee)
        {
            _context.Users.Add(employee);
        }
        void IAdminRepo.UpgradeEmployeeRole(string employeeId, string newRole)
        {
            throw new NotImplementedException();
        }
        public void RemoveEmployee(string employeeId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Ticket Crud
        public IEnumerable<Ticket> DetailsOfTicket(int projectid)
        {
            return _context.Tickets.Where(a => a.ProjectId == projectid);

        }

        public Ticket? DetailsOfTicketwithattachment(int ticketid)
        {
            return _context.Set<Ticket>()
                .Include(a => a.Attachments)
                .Include(c => c.Comments)
                .FirstOrDefault(a => a.TicketId == ticketid);

        }
        #endregion
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

    }
}
