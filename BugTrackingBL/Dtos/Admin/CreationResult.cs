using Microsoft.AspNetCore.Identity;

namespace BugTrackingBL
{
    public class CreationResult
    {
        public string?  userId { get; set; }
        public bool Success { get; set; }
        public IEnumerable<IdentityError>? Errors { get; set; }

    }
}
