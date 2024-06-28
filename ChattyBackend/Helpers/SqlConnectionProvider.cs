using System.Data;
using ChattyBackend.Helpers.Interfaces;
using Npgsql;

namespace ChattyBackend.Helpers;

public sealed class SqlConnectionProvider(IConfiguration configuration) : ISqlConnectionProvider
{
    public async Task<IDbConnection> GetConnection(string name = "default")
    {
        var connString = configuration.GetConnectionString(name);

        if (connString is null)
            throw new ArgumentException(name);

        var conn = new NpgsqlConnection(connString);

        await conn.OpenAsync();

        return conn;
    }
}
