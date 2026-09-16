using System.Data;

namespace Cintrix.Commerce.Api.Infrastructure.Persistence.Connection
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
    }
}
