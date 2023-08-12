using BugTrackingBL;
using BugTrackingBL.Dtos.Developers;
using BugTrackingBL.Dtos.Testers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugTrackingAPI.Controllers
{
    [Authorize(Policy = "Tester")]
    [Route("api/[controller]")]
    [ApiController]
    public class TesterController : ControllerBase
    {
        private readonly ITesterManager testerManager;
        public TesterController(ITesterManager tester)
        {
            testerManager = tester;
        }

        
        [HttpPost]
        [Route("Approve")]
        public IActionResult ApproveTicket(ApproveTicketDto ticket)
        {
            var ApprovedTicket = testerManager.ApproveTicket(ticket);
            if (!ApprovedTicket)
            {
                return BadRequest();
            }
            
            return NoContent();
        }

        [HttpPost]
        [Route("CreateTicket")]
        public IActionResult CreateTicket(CreateTicketDto ticket)
        {
          var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
          var CreateTicket=  testerManager.CreateTicket(ticket,currentuser);
            if (!CreateTicket)
            {
                return BadRequest();
            }
            return NoContent();
        }
        [HttpGet]
        [Route("GetAllTickets/{testerId}")]
        public ActionResult<List<GetTicketsDto>> GetAllTickets(string testerId)
        {
            var tickets = testerManager.GetAllTickets(testerId);
            if(tickets == null)
                return BadRequest();
            return Ok(tickets);
           
        }

        [HttpPost]
        [Route("UpdateTester")]
        async public Task<IActionResult> UpdateTester(TesterUpdateDto tester)
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UpdateTesterData = await testerManager.Update(tester,currentuser);
            if (!UpdateTesterData)
            {
                return BadRequest();
            }           
            return Ok("Tester updated successfully.");          
        }
        [HttpPost]
        [Route("/TesterComment")]
        async public Task<ActionResult> AddComment(CommentAddDto comment)
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await testerManager.AddComment(comment, currentuser);
            if (result)
                return Ok("Comment Added successfully!");
            return BadRequest();
        }
        [HttpPost]
        [Route("TesterImage")]
        async public Task<IActionResult> UpdateTesterImage(IFormFile file)
        {
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var @params = new SavingFileParams
            {
                Host = Request.Host.ToString(),
                Scheme = Request.Scheme.ToString(),
                DirectoryPath = Environment.CurrentDirectory.ToString(),
            };

            var result = await testerManager.UpdateUserImage(file, currentuser, @params);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
