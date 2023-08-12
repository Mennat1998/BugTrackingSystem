using BugTrackingBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingAPI.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager; 
        public AdminController(IAdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        #region Project EndPoints
        [HttpGet]
        [Route("/projects")]
        public ActionResult<List<ProjectReadDto>> GetAllProjects()
        {
            return _adminManager.GetAllProjects().ToList();
        }

        [HttpGet]
        [Route("/projects/{id}")]
        public ActionResult<ProjectDetailsReadDto> GetProjectById(int id)
        {
            var project = _adminManager.GetProjectById(id);
            if(project == null)
            {
                return NotFound();
            }
            return project;
        }
        [HttpPost]
        [Route("/projects")]
        public ActionResult<int> AddProject(AddProjectDto project)
        {
            var newid = _adminManager.AddProject(project);
            return CreatedAtAction(
                nameof(GetProjectById),
                new {id = newid},
                new GeneralResponse("Project Added successfully")
                );
        }
        #endregion

        #region Employees EndPoints
        [HttpGet]
        [Route("/employees")]
        public ActionResult<List<EmployeeReadDto>> GetAllEmployees()
        {
            return _adminManager.GetAllEmployees().ToList();
        }
        [HttpGet]
        [Route("/Managers")]
        public ActionResult<List<EmployeeReadDto>> GetAllManagers()
        {
            return _adminManager.GetAllManagers().ToList();
        }

        [HttpGet]
        [Route("/employee/{userId}")]
        async public Task<ActionResult<EmployeeReadDto?>> GetEmployeeById(string userId) {
            var employee = await _adminManager.GetEmployeeById(userId);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        [Route("/employee")]
        async public Task<ActionResult> AddNewEmployee(EmployeeRegisterDto employee)
        {
            CreationResult result = await _adminManager.AddEmployee(employee);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { userId = result.userId},
                new GeneralResponse("User Added Successfully!")
                );
        }

        [HttpPatch]
        [Route("/employee/{userId}")]
        async public Task<ActionResult> UpgradeUserRole(string userId, string newRole)
        {
            CreationResult result = await _adminManager.UpgradeRole(userId, newRole);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new GeneralResponse("User Role Updated SuccessFully!"));
        }
        [HttpDelete]
        [Route("/employee/{userId}")]
        async public Task<ActionResult> RemoveEmployee(string userId)
        {
            CreationResult result = await _adminManager.RemoveEmployee(userId);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new GeneralResponse("User Deleted SuccessFully!"));
        }
        #endregion

        #region Ticket EndPoints
        [HttpGet]
        [Route("/Tickets/{projectid}")]
        public ActionResult GetTicketsByProjectid(int projectid)
        {
            var ManagerNotification = _adminManager.DetailsOfTicketbyprojectid(projectid);
            return Ok(ManagerNotification);
        }

        [HttpGet]
        [Route("/Ticket/{ticketid}")]
        public ActionResult GetTicketswithattachments(int ticketid)
        {
            var ManagerNotification = _adminManager.DetailsOfTicketWithAttach(ticketid);
            return Ok(ManagerNotification);
        }
        
        #endregion
    }
}
