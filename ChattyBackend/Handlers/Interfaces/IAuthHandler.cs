using ChattyBackend.Models.IncomingRequest;
using ChattyBackend.Models.OutgoingResponses;

namespace ChattyBackend.Handlers.Interfaces;

public interface IAuthHandler
{
    Task<Guid?> Register(RegisterRequest request);
    Task<AuthResponse?> Login(LoginRequest request);
}
