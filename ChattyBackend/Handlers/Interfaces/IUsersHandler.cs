using ChattyBackend.Models.Users;

namespace ChattyBackend.Handlers.Interfaces;

public interface IUsersHandler
{
    Task<OwnUserResponse?> GetOwnUserById(Guid userId);
    Task<PublicUserInfoResponse?> GetPublicUserInfoById(Guid id);
}
