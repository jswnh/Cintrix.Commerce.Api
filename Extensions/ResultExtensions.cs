using Cintrix.Commerce.Api.Domain.Common;

namespace Cintrix.Commerce.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot convert a successful result to ProblemDetails.");
            }

            return Results.Problem(
                statusCode: (int)result.Error.Type,
                title: result.Error.Code,
                detail: result.Error.Description
            );
        }
    }
}
