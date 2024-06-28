namespace ChattyBackend.Models;

public record AuthUser(Guid Id, string Email, string Password, string Username);
