using ChattyBackend.Handlers.Interfaces;
using ChattyBackend.Helpers;
using ChattyBackend.Helpers.Interfaces;
using ChattyBackend.Models;
using ChattyBackend.Models.IncomingRequest;
using ChattyBackend.Models.OutgoingResponses;
using ChattyBackend.Repositories.Interfaces;

namespace ChattyBackend.Handlers;

public sealed class AuthHandler(
    IAuthRepository authRepository,
    IPasswordHasher passwordHasher,
    TokenService tokenService
) : IAuthHandler
{
    private string _signupSecret = File.ReadAllText("toucan2.txt");

    public async Task<bool> Register(RegisterRequest request)
    {
        var (email, password, username, secret) = request;

        var secretIsRight = secret == _signupSecret;
        if (!secretIsRight)
            return false;

        var user = new AuthUser(Guid.NewGuid(), email, password, username);

        var result = await authRepository.RegisterUser(user);

        return result;
    }

    public async Task<AuthResponse?> Login(LoginRequest request)
    {
        var (email, password) = request;
        var user = await authRepository.GetLoginUserByEmail(email);

        if (user is null)
        {
            passwordHasher.Hash("125678");

            return null;
        }

        var verified = passwordHasher.Verify(user.Password, password);

        if (!verified)
        {
            return null;
        }

        var token = tokenService.CreateToken(user);

        return new AuthResponse(token);
    }
}
