using BugTrackingBL.Dtos.Developers;
using BugTrackingBL.Dtos.Manager;
using BugTrackingDAL;
using BugTrackingDAL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BugTrackingBL.Managers.Manager
{
    public class ManagerManager :IManagerManager
    {
        public readonly IManagerRepo _ManagerRepo;
        private readonly UserManager<GeneralUser> _userManager;
        public ManagerManager(IManagerRepo ManagerRepo, UserManager<GeneralUser> userManager)
        {
            _ManagerRepo = ManagerRepo;
            _userManager = userManager;
        }

        public List<DeveloperListDto> GetAllDeveloperNames()
        {
           var ListOfdevelopers= _ManagerRepo.GetAllDevelopersNames();
            return ListOfdevelopers.Select(T => new DeveloperListDto
            {

                DevId=T.Id,
                DevName=T.UserName
            }).ToList();
        }
        async public Task<List<TicketNameDto>> GetAllTickets(string credential)
        {
            var manager = await _userManager.FindByEmailAsync(credential);
            var ListOfTickets = _ManagerRepo.GetAllTickets(manager.Id);
            return ListOfTickets.Select(T => new TicketNameDto
            {
                TicketId=T.TicketId,
                TicketName=T.Title
            }).ToList();
        }


        async public Task<List<ProjectDto>> GetAllProjects(string credential)
        {
            var manager = await _userManager.FindByEmailAsync(credential);
  
            var allprojects =  _ManagerRepo.GetAllProjects(manager.Id);
            return allprojects.Select(T => new ProjectDto
            {
                ProjectId=T.ProjectId,
                ProjectName = T.ProjectName,
                ProjectDescription = T.Description
            }).ToList();
        }

        async public Task<ManagerDetailsDto?> ManagerDetails(string managerId)
        {
            var manager = await _userManager.FindByEmailAsync(managerId);
            return new ManagerDetailsDto
            {
                Name = manager.UserName,
                Address = manager.Address,
                PhoneNumber = manager.PhoneNumber,
                Type = manager.Type,
                Email = manager.Email
            };
        }
         public ProjectDto? GetProject(int ProjectId)
        {
           
            Project? projectwithDevelopers = _ManagerRepo.GetProject(ProjectId);
            Tester? Tester = _ManagerRepo.GetTester(projectwithDevelopers.TesterId);
            if (projectwithDevelopers == null ) { return null; }
            List<Developer> DevelopersinProject = projectwithDevelopers.Developers.ToList();
            List<string> DevIds = new List<string>();
            if (DevelopersinProject != null)
            {
               
                foreach (var i in DevelopersinProject)
                {
                    DevIds.Add(i.UserName);
                }
            }
            return new ProjectDto
            {
                ProjectId=projectwithDevelopers.ProjectId,
                ProjectName = projectwithDevelopers.ProjectName,
                ProjectDescription = projectwithDevelopers.Description,
                Developers = DevIds,
                TesterName = Tester.UserName
            };
        }
        public IEnumerable<TicketDetailsDto> DetailsOfTicketbyprojectid(int projectid)
        {
            var ListFromDb = _ManagerRepo.DetailsOfTicket(projectid);
            return ListFromDb.Select(T => new TicketDetailsDto
            {
                ProjectId = projectid,
                TicketTitle = T.Title,
                TicketDescription = T.Description,
            }).ToList();

        }
        public TicketDetailsDtoWithAttchment? DetailsOfTicketWithAttach(int id)
        {
            var TicketFromDb = _ManagerRepo.DetailsOfTicketwithattachment(id);
            if (TicketFromDb == null) return null;
            
                return new TicketDetailsDtoWithAttchment
                {
                    TicketTitle = TicketFromDb.Title,
                    TicketDescription = TicketFromDb.Description,
                    AttachmentDetails = TicketFromDb.Attachments
                    .Select(D => new AttachmentDto
                    {
                        Type = D.Type
                    }).ToList()
                };
            
            
        }

        public bool AddProjectTeam(int projectid, List<string> DevelopersIds, string TesterId)
        {
            Project GetProjectFromDb = _ManagerRepo.GetProject(projectid);
            if (GetProjectFromDb != null)
            {
                foreach (var developerId in DevelopersIds)
                {
                    var developer = _ManagerRepo.GetDeveloper(developerId);

                    if (developer != null)
                    {
                        GetProjectFromDb.Developers.Add(developer);
                    }
                }
                GetProjectFromDb.TesterId = TesterId;
            }
            _ManagerRepo.AddProjectTeam(GetProjectFromDb);
            _ManagerRepo.SaveChanges();
            return true;
        }
        public bool UpdateProjectTeam(int projectid, List<string> DevelopersIds,string TesterId)
        {
            Project GetProjectFromDb = _ManagerRepo.GetProject(projectid);
            foreach (var developerId in DevelopersIds)
            {
                var developer = _ManagerRepo.GetDeveloper(developerId);

                if (developer != null)
                {
                    GetProjectFromDb.Developers.Add(developer);
                }
            }
            GetProjectFromDb.TesterId = TesterId;
            _ManagerRepo.UpdateProjectTeam(GetProjectFromDb);
            _ManagerRepo.SaveChanges();
            return true;
        }
        public bool assignTicket(string DeveloperId, int TicketId)
        {
            var ticket = _ManagerRepo.GetTicket(TicketId);
            if (ticket != null)
            {
                ticket.DeveloperId = DeveloperId.ToString();
                _ManagerRepo.AssignTicket(ticket);
                _ManagerRepo.SaveChanges();
                return true;
            }
            return false;

        }
        async public Task<bool> AddComment(CommentAddDto comment, string credential)
        {
            var Currentuser = await _userManager.FindByEmailAsync(credential);
            if (Currentuser != null)
            {
                var newComment = new Comments
                {
                    Content = comment.Content,
                    UserId = Currentuser.Id,
                    TickectId = comment.TicketId,
                    UserType = Currentuser.Type
                };
                _ManagerRepo.AddComment(newComment);
                _ManagerRepo.SaveChanges();
                return true;
            }
            return false;
        }


        #region Update Tester Image
        async public Task<bool> UpdateUserImage(IFormFile img, string credential, SavingFileParams savingFileParams)
        {
            var currentUser = await _userManager.FindByEmailAsync(credential);
            if (currentUser == null) { return false; }

            #region Checking Extension

            var extension = Path.GetExtension(img.FileName);

            var allowedExtensions = new string[]
            {
            ".jpg",
            ".svg",
            ".png"
            };

            bool isExtensionAllowed = allowedExtensions.Contains(extension, StringComparer.InvariantCultureIgnoreCase);

            if (!isExtensionAllowed)
            {
                return false;
            }

            #endregion

            #region Checking Length

            bool isSizeAllowed = img.Length > 0 && img.Length <= 4_000_000;

            if (!isSizeAllowed)
            {
                return false;
            }

            #endregion

            #region Storing The File

            var newFileName = $"{Guid.NewGuid()}{extension}";
            var filesPath = Path.Combine(Environment.CurrentDirectory, "Files");
            var fullFilePath = Path.Combine(filesPath, newFileName);

            using var stream = new FileStream(fullFilePath, FileMode.Create);
            img.CopyTo(stream);

            #endregion

            #region Generating URL
            var staticFilesPath = Path.Combine(savingFileParams.DirectoryPath, "Files");
            var url = $"{savingFileParams.Scheme}://{savingFileParams.Host}/Files/{newFileName}";
            #endregion        

            currentUser.Image = url;
            _ManagerRepo.SaveChanges();
            return true;
        }
        #endregion

    }
}
