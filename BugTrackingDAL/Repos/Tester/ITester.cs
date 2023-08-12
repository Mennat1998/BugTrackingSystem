
namespace BugTrackingDAL
{
    public interface ITester
    {
       
        IEnumerable<Ticket> GetAllTickets(string testerId); 
        Ticket? GetTicketById (int ticketId);
        Tester? GetTesterById (string testerId);
        Project? GetProjectById(int id);
        void CreateTicket( Ticket ticket);
        void Update(Tester tester);
        void ApproveTicket( Ticket ticket);
        void AddComment(Comments comment);

        int SaveChanges();
    }
}
