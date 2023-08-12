using BugTrackingBL;
using BugTrackingBL.Dtos.Developers;
using BugTrackingBL.Dtos.Manager;
using BugTrackingBL.Managers.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugTrackingAPI.Controllers
{
    [Authorize(Policy = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        public readonly IManagerManager _Manager;
        public ManagerController(IManagerManager Manager)
        {
            _Manager = Manager;
        }


        [HttpGet]
        [Route("GetAllTickets")]

            async public Task<ActionResult<TicketNameDto>> GetAllTickets()
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Ticketsofmanager = await _Manager.GetAllTickets(currentuser);
            return Ok(Ticketsofmanager);
        }

        [HttpGet]
        [Route("GetAllDevelopersNames")]
        public ActionResult GetAllDevelopersNames()
        {
            return Ok(_Manager.GetAllDeveloperNames() );
        }
        //Get TickerDetails
        [HttpGet]
        [Route("GetTicketsByProjectid/{projectid}")]
        public ActionResult GetTicketsByProjectid(int projectid)
        {
            var ManagerNotification = _Manager.DetailsOfTicketbyprojectid(projectid);
            return Ok(ManagerNotification);
        }

        [HttpGet]
        [Route("GetTicketswithattachments/{ticketid}")]
        public ActionResult GetTicketswithattachments(int ticketid)
        {
            var ManagerNotification = _Manager.DetailsOfTicketWithAttach(ticketid);
            if(ManagerNotification == null) { return BadRequest("Cant Find Ticket"); }
            return Ok(ManagerNotification);
        }
        [HttpGet]
        [Route("GetProject/{project_Id}")]
        public ActionResult GetProject(int project_Id)
        {
      
            var project = _Manager.GetProject(project_Id);
            if (project == null) { return BadRequest("NoProject for that id"); }
            return Ok(project);
        }
        [HttpGet]
        [Route("GetAllProject")]
        async public Task<ActionResult<List<ProjectDto>>> GetAllProjects()
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projectofmanager =await _Manager.GetAllProjects(currentuser);
            if(projectofmanager == null) { return BadRequest("NoProjects For that manager"); }
            return Ok(projectofmanager);
        }
        [HttpGet]
        [Route("ManagerDetails")]
        async public Task<ActionResult<ManagerDetailsDto>> GetManagerDetails()
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var managerDetails = await _Manager.ManagerDetails(currentuser);
            return Ok(managerDetails);
        }

        [HttpPut]
        [Route("UpadteProjectTeam")]
        public ActionResult UpdateProjectTeam(ProjectTeamDto project)
        {
            var teamupdated = _Manager.UpdateProjectTeam(project.ProjectId, project.DevelopersIds, project.TesterId);
            if (teamupdated != true) { return BadRequest("Error Not Updated"); }
            return Ok("Updated");

        }


        [HttpPost]
        [Route("AddProjectTeam")]
        public ActionResult AddProjectTeam(ProjectTeamDto project)
        {
            var teamadded = _Manager.AddProjectTeam(project.ProjectId, project.DevelopersIds, project.TesterId);
            if (teamadded != true) { return BadRequest("Error Not Added"); }
            return Ok("Added Succesfulyy");
        }

        [HttpPost]
        [Route("assignticket")]
        public ActionResult assignTicket(string DeveloperId, int TicketId)
        {
            var ticketassigned = _Manager.assignTicket(DeveloperId, TicketId);
            if (ticketassigned == true) { return Ok(); }
            return BadRequest();
        }
        [HttpPost]
        [Route("ManagerComment")]
        async public Task<ActionResult> AddComment(CommentAddDto comment)
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _Manager.AddComment(comment, currentuser);
            if(result)
            return Ok("Comment Added successfully!");
            return BadRequest();
        }

        [HttpPost]
        [Route("ManagerImage")]
        async public Task<IActionResult> UpdateManagerImage(IFormFile file)
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var @params = new SavingFileParams
            {
                Host = Request.Host.ToString(),
                Scheme = Request.Scheme.ToString(),
                DirectoryPath = Environment.CurrentDirectory.ToString(),
            };

            var result = await _Manager.UpdateUserImage(file, currentuser, @params);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
 