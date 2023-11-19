namespace Auth.Services
{
    public interface IAuthService
    {
        string Authenticate(string userName, string password);
    }
}
