using ChattyBackend.Models;
using ChattyBackend.Models.IncomingRequest;

namespace ChattyBackend.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<bool> RegisterUser(AuthUser user);
    Task<string?> GetPasswordByEmail(string email);
    Task<AuthUser?> GetLoginUserByEmail(string email);
}
