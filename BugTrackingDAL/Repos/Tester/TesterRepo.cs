using Microsoft.EntityFrameworkCore;

namespace BugTrackingDAL
{
    public class TesterRepo : ITester
    {
        private readonly BugContext _context;

        public TesterRepo(BugContext context)
        {
            _context = context;
        }

        public IEnumerable<Ticket> GetAllTickets(string testerId)
        {
            return _context.Tickets.Where(t => t.TesterId == testerId).Include(a=>a.Attachments);
        }
       
        public void CreateTicket( Ticket ticket)
        {
           
           _context.Tickets.Add(ticket);
        }
        public void ApproveTicket( Ticket ticket)
        {
 
        }
        public Project? GetProjectById(int id)
        {
            return _context.Projects.FirstOrDefault(i => i.ProjectId == id);
        }

        public void Update(Tester tester)
        {
           
        }
        public void AddComment(Comments comment)
        {
            _context.Comments.Add(comment);
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Ticket? GetTicketById(int ticketId)
        {
           return _context.Tickets.FirstOrDefault(t=>t.TicketId == ticketId);
        }

        public Tester? GetTesterById(string testerId)
        {
            return _context.Testers.FirstOrDefault(t => t.Id == testerId);
        }
    }
}
