using Microsoft.AspNetCore.Identity;

namespace BugTrackingDAL
{
    public class GeneralUser : IdentityUser
    {
        public string Address { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
