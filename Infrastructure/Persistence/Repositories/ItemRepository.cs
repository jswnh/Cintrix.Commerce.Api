using Cintrix.Commerce.Api.Application.DTOs;
using Cintrix.Commerce.Api.Domain.Entity;
using Cintrix.Commerce.Api.Domain.Repositories;
using Cintrix.Commerce.Api.Infrastructure.Persistence.Connection;
using Dapper;

namespace Cintrix.Commerce.Api.Infrastructure.Persistence.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public async Task<Item> CreateAsync(CreateItemRequest request)
        {
            using var con = await _connectionFactory.CreateConnectionAsync();

            Item item = new(request.Label);

            const string sql = """
                INSERT INTO items (
                    item_id,
                    label,
                    created_at
                ) VALUES (
                    @ItemId,
                    @Label,
                    CreatedAt
                )
                """;

            await con.ExecuteAsync(sql, new
            {
                ItemId = item.ItemId.ToString(),
                item.Label,
                item.CreatedAt
            });

            return item;
        }
    }
}
