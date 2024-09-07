using System.Net;
using ChattyBackend.Handlers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChattyBackend.Controllers;

[ApiController]
[Route("users")]
public sealed class UsersController(IUsersHandler usersHandler) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetByToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type is "id");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var userData = await usersHandler.GetOwnUserById(userId);

        if (userData is null)
            return NotFound();

        return Ok(userData);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPublicUserInfo(Guid id)
    {
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type is "id");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out _))
            return Unauthorized();

        var userData = await usersHandler.GetPublicUserInfoById(id);

        return Ok(userData);
    }
}
