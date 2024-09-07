using ChattyBackend.Models.IncomingRequest;
using ChattyBackend.Models.OutgoingResponses;

namespace ChattyBackend.Handlers.Interfaces;

public interface IAuthHandler
{
    Task<bool> Register(RegisterRequest request);
    Task<AuthResponse?> Login(LoginRequest request);
}
