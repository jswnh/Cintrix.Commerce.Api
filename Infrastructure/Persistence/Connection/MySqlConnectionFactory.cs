using MySqlConnector;
using System.Data;

namespace Cintrix.Commerce.Api.Infrastructure.Persistence.Connection
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public MySqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
