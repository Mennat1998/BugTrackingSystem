using BugTrackingBL.Dtos.Admin;

namespace BugTrackingBL
{
    public interface IAdminManager
    {
        IEnumerable<ProjectReadDto> GetAllProjects();
        ProjectDetailsReadDto? GetProjectById(int projectId);
        int AddProject(AddProjectDto project);
        IEnumerable<EmployeeReadDto> GetAllEmployees();
        IEnumerable<EmployeeReadDto> GetAllManagers();
        Task<EmployeeReadDto?> GetEmployeeById(string id);
        Task<CreationResult> AddEmployee (EmployeeRegisterDto employee);
        Task<CreationResult> UpgradeRole(string employeeId ,string newRole);
        Task<CreationResult> RemoveEmployee(string employeeId);
        IEnumerable<TicketDetailsDto> DetailsOfTicketbyprojectid(int projectid);
        TicketDetailsDtoWithAttchment? DetailsOfTicketWithAttach(int id);
    }
}
