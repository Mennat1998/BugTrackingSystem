namespace BugTrackingBL
{
    public interface ILoginManager
    {
        Task<TokenDto> UserLogin(LoginDto credentials);
    }
}
