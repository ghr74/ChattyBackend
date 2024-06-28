using System.Data;

namespace ChattyBackend.Helpers.Interfaces;

public interface ISqlConnectionProvider
{
    public Task<IDbConnection> GetConnection(string name = "default");
}
