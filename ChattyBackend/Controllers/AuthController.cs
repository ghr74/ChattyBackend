using ChattyBackend.Handlers.Interfaces;
using ChattyBackend.Models.IncomingRequest;
using Microsoft.AspNetCore.Mvc;

namespace ChattyBackend.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController(ILogger<AuthController> logger, IAuthHandler authHandler)
    : Controller
{
    [HttpPost("/register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await authHandler.Register(request);
        return Ok(result);
    }

    // this returns a token
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await authHandler.Login(request);
        return Ok(result);
    }
}
