using System.Security.Claims;

namespace BugTrackingBL
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;

        public string Role { get; set; } 
        public DateTime Expiry { get; set; }
    }
}
