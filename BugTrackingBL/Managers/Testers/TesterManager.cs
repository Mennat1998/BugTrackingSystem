using BugTrackingBL.Dtos.Testers;
using BugTrackingDAL;
using TicketStatus = BugTrackingDAL.TicketStatus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using BugTrackingBL.Dtos.Developers;

namespace BugTrackingBL
{
    public class TesterManager : ITesterManager
    {
        private readonly ITester _testerRepo;
        private readonly UserManager<GeneralUser> _userManager;
        public TesterManager (ITester testerRepo, UserManager<GeneralUser> userManager)
        {
            _testerRepo = testerRepo;
            _userManager = userManager;
        }

        #region Approve Ticket Status

        public bool ApproveTicket( ApproveTicketDto ticket)
        {
            var TicketfromDB = _testerRepo.GetTicketById(ticket.Id);
            if(TicketfromDB == null)
            {
                return false;
            }
            TicketfromDB.Status = (TicketStatus)ticket.Status;
            _testerRepo.ApproveTicket(TicketfromDB);
            _testerRepo.SaveChanges();
            return true;
            
        }
        #endregion

        #region Create Ticket 
        public bool CreateTicket(CreateTicketDto ticket,string testerid)
        {
            var project = _testerRepo.GetProjectById(ticket.ProjectId);
            Ticket _ticket = new Ticket 
            {
                Title = ticket.Title,
                Description = ticket.Description,
                Status = TicketStatus.InProgress,
                ProjectId = ticket.ProjectId,
                TesterId = testerid,
                ManagerId = project.MangerId
            };

            _testerRepo.CreateTicket(_ticket);
            _testerRepo.SaveChanges();
            return true;
        }
        #endregion

        #region Get All The Tickets Owned By Tester
        public IEnumerable<GetTicketsDto> GetAllTickets(string testerId)
        {
            var ListFromdB = _testerRepo.GetAllTickets(testerId);
           
            return ListFromdB.Select(t => new GetTicketsDto
            {
                Id = t.TicketId,
                Title = t.Title,
                Description = t.Description,
                Status = (Dtos.Testers.TicketStatus)(TicketStatus)t.Status,
                Attachments = t.Attachments.Select(a => new AttachmentDto
                {
                    Type = a.Type,
                    URL = a.Url
                }).ToList()

            }).ToList();
            
           
        }
        #endregion

        #region Update Tester Data
        async public Task<bool> Update(TesterUpdateDto tester, string credential)
        {
            var testerFromDB = await _userManager.FindByEmailAsync(credential) as Tester;
            if (testerFromDB == null)
            {
                return false;
            }
            //testerFromDB.Image = tester.Image;
            testerFromDB.UserName = tester.Name;
            testerFromDB.Address = tester.Address;
            testerFromDB.Email = tester.Email;
            _testerRepo.Update(testerFromDB);
            _testerRepo.SaveChanges();
            return true;
        }
        #endregion

        #region Add Comment
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
                _testerRepo.AddComment(newComment);
                _testerRepo.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion

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
            _testerRepo.SaveChanges();
            return true;
        }
        #endregion
    }
    }
