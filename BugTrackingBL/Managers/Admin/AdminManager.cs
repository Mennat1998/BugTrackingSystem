using BugTrackingBL.Dtos.Admin;
using BugTrackingDAL;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BugTrackingBL
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepo _AdminRepo;
        private readonly UserManager<GeneralUser> _userManager;
        public AdminManager(IAdminRepo adminrepo, UserManager<GeneralUser> userManager) 
        {
            _AdminRepo = adminrepo;   
            _userManager = userManager;
        }

        #region Project 
        public IEnumerable<ProjectReadDto> GetAllProjects()
        {
            var projects = _AdminRepo.GetAllProjects();
            return projects.Select(p => new ProjectReadDto
            {
                ProId = p.ProjectId,
                Name = p.ProjectName,
                Description = p.Description,
                DevelopersNo = p.Developers.Count, 
                TicketsNo = p.Tickets.Count,
            });
        }
        public ProjectDetailsReadDto? GetProjectById(int projectId)
        {
            Project? project = _AdminRepo.GetProjectById(projectId);
            var managerName = _AdminRepo.GetEmployeeById(project.MangerId).UserName;
            var tester = _AdminRepo.GetEmployeeById(project.TesterId) ;

            if (project is null)
            {
                return null;
            }

            string testerName = "";
            if(tester != null)
            {
                testerName=tester.UserName;
            }

            return new ProjectDetailsReadDto
            {
                ProId = project.ProjectId,
                Name = project.ProjectName,
                Description = project.Description,
                ManagerName = managerName,
                TesterName = testerName,
                DevelopersNames = project.Developers?.Select(p=>p.UserName).ToList(), 
                TicketsNo = project.Tickets.Count
            };
        }
        public int AddProject(AddProjectDto project)
        {
            Project newProject = new Project
            {
                ProjectName = project.Name,
                Description = project.Description,
                MangerId = project.ManagerId
            };
            _AdminRepo.AddProject(newProject);
            _AdminRepo.SaveChanges();
            return newProject.ProjectId;
        }
        #endregion

        #region Employees
        public IEnumerable<EmployeeReadDto> GetAllEmployees()
        {
            return _AdminRepo.GetAllEmployees().Select( e => new EmployeeReadDto
            {
                Id = e.Id,
                Name = e.UserName,
                Role = e.Type
            });
        }
        public IEnumerable<EmployeeReadDto> GetAllManagers()
        {
            return _AdminRepo.GetAllEmployees().Where(a => a.Type == "Manager").Select(e => new EmployeeReadDto
            {
                Id = e.Id,
                Name = e.UserName,
                Role = e.Type
            });
        }
        async public Task<EmployeeReadDto?> GetEmployeeById(string id)
        {
            var employee = _AdminRepo.GetEmployeeById(id);
            if (employee == null)
                return null;
            var role = await _userManager.GetRolesAsync(employee);
            return new EmployeeReadDto 
            {
                Id = employee.Id,
                Name = employee.UserName, 
                Role = role.SingleOrDefault(),
            };
        }
        async public Task<CreationResult> AddEmployee(EmployeeRegisterDto employee)
        {
            GeneralUser user = CreateUser(employee);

            var creationResult = await _userManager.CreateAsync(user,
                employee.Password);

            if (creationResult.Succeeded)
            {
                #region generate user claim list
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role),
                new Claim("Nationality","Egyptian")
                };
                #endregion

                await _userManager.AddClaimsAsync(user, claims);

                #region Add User Role
                await _userManager.AddToRoleAsync(user, employee.Role);
                user.Type = employee.Role;
                await _userManager.UpdateAsync(user);
                #endregion

                return new CreationResult{
                    Success = true,
                    userId = user.Id,
                };
            }
            return new CreationResult
            {
                Success = false,
                Errors = creationResult.Errors
            };
        }
        async public Task<CreationResult> UpgradeRole(string employeeId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(employeeId.ToString());

            if (user == null)
            {
                return new CreationResult { Success = false, Errors = new List<IdentityError> { new IdentityError { Description = "User not found" } } };
            }

            var currentRole = (await _userManager.GetRolesAsync(user)).SingleOrDefault();

            if (currentRole == newRole)
            {
                return new CreationResult { Success = false, Errors = new List<IdentityError> { new IdentityError { Description = "User is already in the specified role" } } };
            }

            if (currentRole != null)
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRole);

                if (!removeResult.Succeeded)
                {
                    return new CreationResult { Success = false, Errors = removeResult.Errors };
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            user.Type = newRole;
            await _userManager.UpdateAsync(user);

            if (!addResult.Succeeded)
            {
                return new CreationResult { Success = false, Errors = addResult.Errors };
            }

            return new CreationResult { Success = true };
        }
        async public Task<CreationResult> RemoveEmployee(string employeeId)
        {
            return await UpgradeRole(employeeId, "Unauthorized");          
        }
        #endregion

        #region Ticket
        public IEnumerable<TicketDetailsDto> DetailsOfTicketbyprojectid(int projectid)
        {
            var ListFromDb = _AdminRepo.DetailsOfTicket(projectid);
            return ListFromDb.Select(T => new TicketDetailsDto
            {
                TicketTitle = T.Title,
                TicketDescription = T.Description,
            }).ToList();

        }
        public TicketDetailsDtoWithAttchment? DetailsOfTicketWithAttach(int id)
        {
            var TicketFromDb = _AdminRepo.DetailsOfTicketwithattachment(id);
            return new TicketDetailsDtoWithAttchment
            {
                TicketTitle = TicketFromDb.Title,
                TicketDescription = TicketFromDb.Description,
                AttachmentDetails = TicketFromDb.Attachments
                .Select(D => new AttachmentDto
                {
                    Type = D.Type
                }).ToList(),
                CommentsDetails = TicketFromDb.Comments
                .Select(c => new CommentReadDto
                {
                    UserName = _AdminRepo.GetEmployeeById(c.UserId).UserName,
                    UserType = _AdminRepo.GetEmployeeById(c.UserId).Type,
                    Content = c.Content
                }).ToList()
            };
        }
        #endregion

        #region Helpers
        private static GeneralUser CreateUser(EmployeeRegisterDto newUser)
        {
            GeneralUser user;
            switch (newUser.Role)
            {
                case "Developer":
                    user = new Developer();
                    break;
                case "Tester":
                    user = new Tester();
                    break;
                case "Manager":
                    user = new Manager();
                    break;
                default:
                    user = new();
                    break;
            }

            user.UserName = newUser.FirstName + "_" + newUser.LastName;
            user.Email = newUser.Email;
            user.PhoneNumber = newUser.PhoneNumber;
            user.Address = newUser.Address;
            user.Type = newUser.Role;
            return user;
        }
        #endregion
    }
}
