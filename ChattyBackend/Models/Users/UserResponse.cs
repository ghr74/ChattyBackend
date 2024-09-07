namespace ChattyBackend.Models.Users;

public sealed record OwnUserResponse(Guid Id, string Email, string Username);

public sealed record PublicUserInfoResponse(Guid Id, string Username);
