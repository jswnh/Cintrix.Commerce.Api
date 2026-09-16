using Cintrix.Commerce.Api.Application.DTOs;
using Cintrix.Commerce.Api.Domain.Common;
using Cintrix.Commerce.Api.Domain.Entities;

namespace Cintrix.Commerce.Api.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Result<Item>> CreateAsync(Item item, CancellationToken ct = default);
        Task<Result<Item>> GetByIdAsync(Guid itemId, CancellationToken ct = default);
        Task<Result<IReadOnlyList<Item>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<Item>> UpdateAsync(Guid ItemId, UpdateItemRequest requestItem, CancellationToken ct = default);
        Task<Result> DeleteAsync(Guid itemId, CancellationToken cancellationToken = default);
    }
}
