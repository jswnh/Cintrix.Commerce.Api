using Cintrix.Commerce.Api.Application.DTOs;
using Cintrix.Commerce.Api.Domain.Entity;

namespace Cintrix.Commerce.Api.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Item> CreateAsync(CreateItemRequest request);
    }
}
