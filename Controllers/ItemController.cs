using Cintrix.Commerce.Api.Application.DTOs;
using Cintrix.Commerce.Api.Domain.Common;
using Cintrix.Commerce.Api.Domain.Entities;
using Cintrix.Commerce.Api.Domain.Repositories;
using Cintrix.Commerce.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Cintrix.Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IItemRepository repository) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Item), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Create([FromBody] CreateItemRequest request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Label))
            {
                return Result.Failure(Error.Validation("Items.EmptyLabel", "Item label cannot be empty or whitespace."))
                             .ToProblemDetails();
            }

            if (request.Label.Length > 100)
            {
                return Result.Failure(Error.Validation("Items.LabelTooLong", "Item label cannot exceed 100 characters."))
                             .ToProblemDetails();
            }

            var item = new Item(label: request.Label);

            Result<Item> result = await repository.CreateAsync(item, ct);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.CreatedAtRoute(
                routeName: nameof(GetById),
                routeValues: new { id = result.Value.ItemId },
                value: result.Value
            );
        }

        [HttpGet("{id:guid}", Name = nameof(GetById))]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IResult> GetById(Guid id, CancellationToken ct)
        {
            Result<Item> result = await repository.GetByIdAsync(id, ct);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.Ok(result.Value);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IResult> GetAll(CancellationToken ct)
        {
            Result<IReadOnlyList<Item>> result = await repository.GetAllAsync(ct);

            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.Ok(result.Value);
        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Update(
            Guid id,
            [FromBody] UpdateItemRequest request,
            CancellationToken ct)
        {
            if (request.Label is null)
            {
                return Result.Failure(Error.Validation(
                    "Items.EmptyUpdate",
                    "At least one field must be provided to update."))
                    .ToProblemDetails();
            }

            if (request.Label is not null)
            {
                if (string.IsNullOrWhiteSpace(request.Label))
                {
                    return Result.Failure(Error.Validation("Items.EmptyLabel", "Item label cannot be empty or whitespace."))
                                 .ToProblemDetails();
                }

                if (request.Label.Length > 250)
                {
                    return Result.Failure(Error.Validation("Items.LabelTooLong", "Item label cannot exceed 250 characters."))
                                 .ToProblemDetails();
                }
            }

            Result<Item> result = await repository.UpdateAsync(id, request, ct);

            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Delete(Guid id, CancellationToken ct)
        {
            Result result = await repository.DeleteAsync(id, ct);

            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.NoContent();
        }
    }
}
