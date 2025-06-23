

namespace MemoriaAPI.Service
{
    public interface IAuthService
    {
        string? Authenticate(string username, string password);
    }
}
