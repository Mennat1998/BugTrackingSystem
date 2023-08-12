using BugTrackingBL;
using BugTrackingBL.Dtos.Developers;
using BugTrackingBL.Managers.Developers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugTrackingAPI.Controllers
{
    [Authorize(Policy = "Developer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperManager _developer;

        public DeveloperController(IDeveloperManager developer)
        {
            _developer = developer;
        }

        [HttpPost]
        [Route("HandleTickets/{ticketId}")]
        public IActionResult HandleTickets(IFormFile file,int ticketId)
        {
            var @params = new SavingFileParams
            {
                Host = Request.Host.ToString(),
                Scheme = Request.Scheme.ToString(),
                DirectoryPath = Environment.CurrentDirectory.ToString(),
            };
            
            var handleTicket = _developer.HandleTicket(ticketId, file,@params);

            if (!handleTicket)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateDeveloper")]
        public IActionResult UpdateDeveloper(DeveloperUpdateDto developerDto)
        {
            var UpdateDeveloper = _developer.Update(developerDto);
            if (!UpdateDeveloper)
            {
                return BadRequest();
            }
            return Ok("Developer updated successfully.");
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetTicketsByDeveloperId(string Id)
        { 
            var GetTickets = _developer.GetTicketsByDeveloperId(Id);
            if (GetTickets == null)
            {
                return BadRequest();
            }
            return Ok(GetTickets);
        }

        [HttpPost]
        [Route("/DeveloperComment")]
        async public Task<ActionResult> AddComment(CommentAddDto comment)
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _developer.AddComment(comment, currentuser);
            if (result)
                return Ok("Comment Added successfully!");
            return BadRequest();
        }

        [HttpPost]
        [Route("DeveloperImage")]
        async public Task<IActionResult> UpdateDeveloperImage(IFormFile file)
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var @params = new SavingFileParams
            {
                Host = Request.Host.ToString(),
                Scheme = Request.Scheme.ToString(),
                DirectoryPath = Environment.CurrentDirectory.ToString(),
            };

            var result = await _developer.UpdateUserImage(file, currentuser, @params);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
