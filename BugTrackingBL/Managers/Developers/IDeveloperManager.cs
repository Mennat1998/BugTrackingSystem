using BugTrackingBL.Dtos.Developers;
using Microsoft.AspNetCore.Http;

namespace BugTrackingBL.Managers.Developers
{
    public interface IDeveloperManager
    {
        IEnumerable<GetTicketDeveloperDto> GetTicketsByDeveloperId(string developerId);
        bool Update(DeveloperUpdateDto developer);
        bool HandleTicket(int ticketId, IFormFile file, SavingFileParams savingFileParams);
        Task<bool> AddComment(CommentAddDto comment, string credential);
        Task<bool> UpdateUserImage(IFormFile img, string credential, SavingFileParams savingFileParams);
    }
}

