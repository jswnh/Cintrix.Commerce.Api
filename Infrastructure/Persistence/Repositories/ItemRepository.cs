using Cintrix.Commerce.Api.Application.DTOs;
using Cintrix.Commerce.Api.Domain.Common;
using Cintrix.Commerce.Api.Domain.Entities;
using Cintrix.Commerce.Api.Domain.Repositories;
using Cintrix.Commerce.Api.Infrastructure.Persistence.Connection;
using Dapper;
using MySqlConnector;

namespace Cintrix.Commerce.Api.Infrastructure.Persistence.Repositories
{
    public class ItemRepository(IDbConnectionFactory connectionFactory) : IItemRepository
    {

        public async Task<Result<Item>> CreateAsync(Item request, CancellationToken ct)
        {
            const string sql = """
                INSERT INTO items (
                    item_id,
                    label,
                    created_at
                ) VALUES (
                    @ItemId,
                    @Label,
                    @CreatedAt
                )
            """;
            try
            {
                using var connection = await connectionFactory.CreateConnectionAsync(ct);
                var command = new CommandDefinition(
                    sql,
                    new
                    {
                        ItemId = request.ItemId.ToString(),
                        request.Label,
                        CreatedAt = request.CreatedAt.UtcDateTime,
                    },
                    cancellationToken: ct
                );
                await connection.ExecuteAsync(command);
                return request;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return Error.Conflict("Items.DuplicateKey", "An item with this identifier already exists.");
            }
            catch (Exception ex)
            {
                return Error.Failure("Database.Error", ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(Guid itemId, CancellationToken cancellationToken = default)
        {
            const string sql = """
            DELETE FROM items 
            WHERE item_id = @ItemId;
            """;

            try
            {
                using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

                var command = new CommandDefinition(
                    sql,
                    new { ItemId = itemId.ToString() },
                    cancellationToken: cancellationToken
                );

                int rowsAffected = await connection.ExecuteAsync(command);

                if (rowsAffected == 0)
                {
                    return Error.NotFound("Items.NotFound", $"Item with ID '{itemId}' was not found.");
                }

                return Result.Success();
            }
            catch (OperationCanceledException)
            {
                return Error.Failure("Request.Canceled", "The delete operation was canceled.");
            }
            catch (Exception ex)
            {
                return Error.Failure("Database.Error", ex.Message);
            }
        }

        public async Task<Result<IReadOnlyList<Item>>> GetAllAsync(CancellationToken ct)
        {
            const string sql = """
                SELECT
                    item_id as ItemId,
                    label as Label,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                FROM items
                ORDER BY created_at DESC;
            """;

            try
            {
                using var connection = await connectionFactory.CreateConnectionAsync(ct);
                var command = new CommandDefinition(sql, cancellationToken: ct);
                IEnumerable<Item> items = await connection.QueryAsync<Item>(command);
                return items.ToList().AsReadOnly();
            }
            catch (OperationCanceledException)
            {
                return Error.Failure("Request.Canceled", "The operation was canceled.");
            }
            catch(Exception ex)
            {
                return Error.Failure("Database.Error", ex.Message);
            }
        }

        public async Task<Result<Item>> GetByIdAsync(Guid itemId, CancellationToken ct)
        {
            const string sql = """
                SELECT
                    item_id as ItemId,
                    label as Label,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                FROM items
                WHERE item_id = @ItemId;
            """;

            try
            {
                using var connection = await connectionFactory.CreateConnectionAsync(ct);

                var command = new CommandDefinition(sql, new { ItemId = itemId.ToString() }, cancellationToken: ct);

                Item? item = await connection.QuerySingleOrDefaultAsync<Item>(command);

                if (item is null)
                {
                    return Error.NotFound("Items.NotFound", $"Item with ID '{itemId}' was not found.");
                }

                return item;
            }
            catch (OperationCanceledException)
            {
                return Error.Failure("Request.Canceled", "The operation was canceled.");
            }
            catch (Exception ex)
            {
                return Error.Failure("Database.Error", ex.Message);
            }
        }

        public async Task<Result<Item>> UpdateAsync(Guid itemId, UpdateItemRequest requestItem, CancellationToken ct = default)
        {
            var setClauses = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("ItemId", itemId.ToString());

            if (requestItem.Label is not null)
            {
                setClauses.Add("label = @Label");
                parameters.Add("Label", requestItem.Label.Trim());
            }

            if (setClauses.Count == 0)
            {
                return await GetByIdAsync(itemId, ct);
            }

            setClauses.Add("updated_at = @UpdatedAt");
            parameters.Add("UpdatedAt", DateTimeOffset.UtcNow.UtcDateTime);

            string updateSql = $"""
                UPDATE items
                SET {string.Join(", ", setClauses)}
                WHERE item_id = @ItemId;
                """;

                    const string selectSql = """
                SELECT 
                    item_id AS ItemId,
                    label AS Label,
                    created_at AS CreatedAt,
                    updated_at AS UpdatedAt
                FROM items
                WHERE item_id = @ItemId;
            """;

            try
            {
                using var connection = await connectionFactory.CreateConnectionAsync(ct);
                var updateCommand = new CommandDefinition(
                    updateSql,
                    parameters,
                    cancellationToken: ct
                );

                int rowsAffected = await connection.ExecuteAsync(updateCommand);

                if (rowsAffected == 0)
                {
                    return Error.NotFound("Items.NotFound", $"Item with ID '{itemId}' was not found.");
                }

                var selectCommand = new CommandDefinition(
                    selectSql,
                    new { ItemId = itemId.ToString() },
                    cancellationToken: ct
                );

                Item? updatedItem = await connection.QuerySingleOrDefaultAsync<Item>(selectCommand);

                if (updatedItem is null)
                {
                    return Error.NotFound("Items.NotFound", $"Item with ID '{itemId}' was not found.");
                }

                return updatedItem;
            }
            catch (OperationCanceledException)
            {
                return Error.Failure("Request.Canceled", "The update operation was canceled.");
            }
            catch (Exception ex)
            {
                return Error.Failure("Database.Error", ex.Message);
            }
        }

    }
}
