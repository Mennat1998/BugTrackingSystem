namespace BugTrackingDAL
{
    public interface IDeveloper
    {     
        IEnumerable<Ticket> GetTicketsByDeveloperId(string developerId);
        Developer? GetDeveloperById(string DeveloperId);
        Ticket? GetTicketById(int TicketId);
        void HandleTicket(Ticket ticket); 
        void Update(Developer developer);
        void AddComment(Comments comment);
        int SaveChanges();
       
    }
}






