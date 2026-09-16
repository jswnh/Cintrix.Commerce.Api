namespace Cintrix.Commerce.Api.Domain.Common
{
    public enum ErrorType
    {
        Failure = 500,
        Validation = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409
    }

    public record Error(string Code, string Description, ErrorType Type)
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided.", ErrorType.Failure);

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        public static Error Validation(string code, string description) =>
            new(code, description, ErrorType.Validation);

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);

        public static Error Unauthorized(string code, string description) =>
            new(code, description, ErrorType.Unauthorized);

        public static Error Forbidden(string code, string description) =>
            new(code, description, ErrorType.Forbidden);
    }
}
