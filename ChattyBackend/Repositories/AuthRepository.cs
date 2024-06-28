using System.Diagnostics;
using ChattyBackend.Helpers.Interfaces;
using ChattyBackend.Models;
using ChattyBackend.Repositories.Interfaces;
using Dapper;

namespace ChattyBackend.Repositories;

public sealed class AuthRepository(
    IPasswordHasher passwordHasher,
    ISqlConnectionProvider sqlConnectionProvider
) : IAuthRepository
{
    public async Task<bool> RegisterUser(AuthUser user)
    {
        var hashedUser = user with { Password = passwordHasher.Hash(user.Password) };
        const string query = """
            INSERT INTO Users (Id, Email, Username, Password)
            SELECT @Id, @Email, @Username, @Password
            WHERE NOT EXISTS (
                SELECT 1 FROM Users WHERE Username = @Username OR Email = @Email
            )
            """;

        using var conn = await sqlConnectionProvider.GetConnection();

        var result = await conn.ExecuteAsync(query, hashedUser);

        return result > 0;
    }

    public async Task<string?> GetPasswordByEmail(string email)
    {
        const string query = """
            SELECT password
            FROM users
            WHERE email = @email
            """;

        using var conn = await sqlConnectionProvider.GetConnection();

        var result = await conn.ExecuteScalarAsync<string>(query, new { email });

        return result;
    }

    public async Task<AuthUser?> GetLoginUserByEmail(string email)
    {
        const string query = """
            SELECT id, email, password, username
            FROM users
            WHERE email = @email
            LIMIT 2
            """;

        using var conn = await sqlConnectionProvider.GetConnection();

        var resultEnum = await conn.QueryAsync<AuthUser>(query, new { email });
        var result = resultEnum.AsList();

        // TODO: log an error here
        Debug.Assert(result.Count == 1);

        return result.FirstOrDefault();
    }
}
