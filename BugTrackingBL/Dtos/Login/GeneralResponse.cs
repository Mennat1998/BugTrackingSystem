namespace BugTrackingBL
{
    public class GeneralResponse
    {
        public string Message { get; set; } = string.Empty;
        public GeneralResponse(string message)
        {
            Message = message;
        }
    }
}
