
using Azure.Core;
using BugTrackingBL.Dtos.Developers;
using BugTrackingBL.Dtos.Testers;
using BugTrackingDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BugTrackingBL.Managers.Developers
{
    public class DeveloperManager : IDeveloperManager
    {
        private readonly IDeveloper _developerRepo;
        private readonly SavingFileParams _savingFileParams;
        private readonly UserManager<GeneralUser> _userManager;

        public DeveloperManager(IDeveloper developerRepo, UserManager<GeneralUser> userManager)
        {
            _developerRepo = developerRepo;
            _userManager = userManager;
        }

        #region Get Tickets That Assigned To Developer By Id
        public IEnumerable<GetTicketDeveloperDto> GetTicketsByDeveloperId(string developerId)
        {       
            var tickets = _developerRepo.GetTicketsByDeveloperId(developerId);
            var ticketDtos = new List<GetTicketDeveloperDto>();
            foreach (var ticket in tickets)
            {
                var ticketDto = new GetTicketDeveloperDto
                {
                    Title = ticket.Title,
                    Description = ticket.Description,
                    Status = (Dtos.Developers.TicketStatus)ticket.Status,
                    Attachments =ticket.Attachments.Select(a=>
                    new AttachmentDto
                    {
                        Type = a.Type,
                        URL = a.Url,
                    }).ToList(),
                };
                ticketDtos.Add(ticketDto);
            }
            return ticketDtos;
        }
        #endregion

        #region Update Developer Data
        public bool Update(DeveloperUpdateDto developer)
        {
            Developer? developerFromDB = _developerRepo.GetDeveloperById(developer.Id);
                if (developerFromDB == null)
                {
                return false;
                }
                developerFromDB.Address= developer.Address;
                developerFromDB.UserName = developer.UserName;
                developerFromDB.Email = developer.Email;
            _developerRepo.Update(developerFromDB);
            _developerRepo.SaveChanges();
                return true;
        }
        #endregion

        #region File Manupilation and Hnadling the Ticket
        public bool HandleTicket(int ticketId, IFormFile file, SavingFileParams savingFileParams)
        {
            Ticket? ticketFromDB = _developerRepo.GetTicketById(ticketId);

            if (ticketFromDB == null)
            {
                return false;
            }

            if (ticketFromDB.Status == BugTrackingDAL.TicketStatus.InProgress && file != null)
            {
                ticketFromDB.Status = BugTrackingDAL.TicketStatus.Completed;

                #region Checking Extension

                var extension = Path.GetExtension(file.FileName);

                // TODO: It's better to be part of appsettings.json
                var allowedExtensions = new string[]
                {
            ".txt",
            ".pdf",
            ".pptx",
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

                bool isSizeAllowed = file.Length > 0 && file.Length <= 4_000_000;

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
                file.CopyTo(stream);

                #endregion

                #region Generating URL

                var staticFilesPath = Path.Combine(savingFileParams.DirectoryPath, "Files");
                var url = $"{savingFileParams.Scheme}://{savingFileParams.Host}/Files/{newFileName}";

                #endregion

                var attachmentModel = new Attachments
                {
                    CreatedDate = DateTime.Now,
                    Type = "file",
                    Url = url,      
                };
                ticketFromDB.Attachments.Add(attachmentModel);
            }
            _developerRepo.HandleTicket(ticketFromDB);
            _developerRepo.SaveChanges();
            return true;
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
                _developerRepo.AddComment(newComment);
                _developerRepo.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region Update Developer Image
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
            _developerRepo.SaveChanges();
            return true;
        }
        #endregion
    }
}
