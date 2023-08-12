using BugTrackingBL.Dtos.Developers;
using BugTrackingBL.Dtos.Manager;
using Microsoft.AspNetCore.Http;

namespace BugTrackingBL.Managers.Manager
{
    public interface IManagerManager
    {
        Task<List<ProjectDto>> GetAllProjects(string credential);

        List<DeveloperListDto> GetAllDeveloperNames();
        Task<List<TicketNameDto>> GetAllTickets(string credential);
        
        ProjectDto? GetProject(int ProjectId);
        IEnumerable<TicketDetailsDto> DetailsOfTicketbyprojectid(int projectid);
        TicketDetailsDtoWithAttchment? DetailsOfTicketWithAttach(int id);
        bool AddProjectTeam(int projectid, List<string>DevelopersIds, string TesterId);
        bool UpdateProjectTeam(int projectid, List<string> DevelopersIds, string TesterId);
        bool assignTicket(string DeveloperId, int TicketId);
        Task<bool> AddComment(CommentAddDto comment, string credential);
        Task<ManagerDetailsDto?> ManagerDetails(string managerId);
        Task<bool> UpdateUserImage(IFormFile img, string credential, SavingFileParams savingFileParams);

    }
}
