using BugTrackingBL.Dtos.Developers;
using BugTrackingBL.Dtos.Testers;
using Microsoft.AspNetCore.Http;

namespace BugTrackingBL
{
    public interface ITesterManager
    {
        IEnumerable<GetTicketsDto> GetAllTickets(string testerId); 
        bool CreateTicket(CreateTicketDto ticket, string testerId);
        Task<bool> Update(TesterUpdateDto tester, string testerId);
        bool ApproveTicket( ApproveTicketDto ticket);
        Task<bool> AddComment(CommentAddDto comment, string credential);
        Task<bool> UpdateUserImage(IFormFile img, string credential, SavingFileParams savingFileParams);
    }
}
