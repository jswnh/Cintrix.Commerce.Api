using Cintrix.Commerce.Api.Application.DTOs;

namespace Cintrix.Commerce.Api.Domain.Repositories
{
    public interface IItemVariantRepository
    {
        Task CreateManyAsync(IReadOnlyList<CreateItemVariant> variants, CancellationToken ct = default);
    }
}
