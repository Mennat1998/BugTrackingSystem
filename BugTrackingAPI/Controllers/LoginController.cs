using BugTrackingBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager _loginManager; 

        public LoginController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDto credentials)
        {
            var token = await _loginManager.UserLogin(credentials);
            if (token == null)
            {
                return BadRequest(new GeneralResponse("Wrong Password!"));
            }
            return Ok(token);
        }
    }
}
