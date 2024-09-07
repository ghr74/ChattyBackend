using ChattyBackend.Handlers.Interfaces;
using ChattyBackend.Models.IncomingRequest;
using Microsoft.AspNetCore.Mvc;

namespace ChattyBackend.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(ILogger<AuthController> logger, IAuthHandler authHandler)
    : Controller
{
    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await authHandler.Register(request);
        return result ? Created() : Conflict();
    }

    // this returns a token
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authHandler.Login(request);
        return Ok(result);
    }
}
