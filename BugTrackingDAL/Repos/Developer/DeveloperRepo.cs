using Microsoft.EntityFrameworkCore;

namespace BugTrackingDAL
{
    public class DeveloperRepo : IDeveloper
    {
        private readonly BugContext _context;

        public DeveloperRepo(BugContext context)
        {
            _context = context;
        }

        

        public IEnumerable<Ticket> GetTicketsByDeveloperId(string developerId)
        {
            return _context.Tickets.Include(a => a.Attachments).Where(t => t.DeveloperId == developerId);
        }

        public void Update(Developer developer)
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

        
        public void HandleTicket(Ticket ticket)
        {

        }

        public Developer? GetDeveloperById(string DeveloperId)
        {
            return _context.Developers.FirstOrDefault(t => t.Id == DeveloperId);
            
        }
        public Ticket? GetTicketById(int TicketId)
        {
            return _context.Tickets.FirstOrDefault(t => t.TicketId == TicketId);
        }

       
    }
}
