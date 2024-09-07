using ChattyBackend.Handlers.Interfaces;
using ChattyBackend.Helpers.Interfaces;
using ChattyBackend.Models.Users;
using Dapper;

namespace ChattyBackend.Handlers;

public sealed class UsersHandler(IUsersRepository usersRepository) : IUsersHandler
{
    public Task<OwnUserResponse?> GetOwnUserById(Guid userId) =>
        usersRepository.GetOwnUserById(userId);

    public Task<PublicUserInfoResponse?> GetPublicUserInfoById(Guid id) =>
        usersRepository.GetPublicUserInfoById(id);
}

public interface IUsersRepository
{
    Task<OwnUserResponse?> GetOwnUserById(Guid userId);
    Task<PublicUserInfoResponse?> GetPublicUserInfoById(Guid id);
}

public sealed class UsersRepository(ISqlConnectionProvider sqlConnectionProvider) : IUsersRepository
{
    public async Task<OwnUserResponse?> GetOwnUserById(Guid userId)
    {
        const string query = """
            SELECT id, email, username
            FROM users
            WHERE id = @userId
            LIMIT 1
            """;

        using var conn = await sqlConnectionProvider.GetConnection();

        var result = await conn.QuerySingleOrDefaultAsync<OwnUserResponse>(query, new { userId });

        return result;
    }

    public async Task<PublicUserInfoResponse?> GetPublicUserInfoById(Guid id)
    {
        const string query = """
            SELECT id, username
            FROM users
            WHERE id = @id
            LIMIT 1
            """;

        using var conn = await sqlConnectionProvider.GetConnection();

        var result = await conn.QuerySingleOrDefaultAsync<PublicUserInfoResponse>(
            query,
            new { id }
        );

        return result;
    }
}
