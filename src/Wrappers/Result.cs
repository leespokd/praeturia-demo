namespace Praetura_demo.Wrappers
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorMessage { get; }
        public ErrorCode? Code { get; }

        protected Result(bool isSuccess, string? errorMessage, ErrorCode? code = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Code = code;
        }

        // ----- Static constructors -----

        public static Result Success() => new Result(true, null);
        public static Result Failure(string message) => new Result(false, message);
        public static Result Failure(ErrorCode code, string message) => new Result(false, message, code);

        // ----- Combine multiple results -----

        public static Result Combine(params Result[] results)
        {
            var failed = results.FirstOrDefault(r => r.IsFailure);
            return failed ?? Success();
        }
    }

    /// <summary>
    /// Generic version of Result that carries a value of type T.
    /// </summary>
    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T? value, bool isSuccess, string? errorMessage, ErrorCode? code = null)
            : base(isSuccess, errorMessage, code)
        {
            Value = value;
        }

        // ----- Static constructors -----

        public static Result<T> Success(T value) => new Result<T>(value, true, null);
        public static new Result<T> Failure(string message) => new Result<T>(default, false, message);
        public static Result<T> Failure(ErrorCode code, string message) => new Result<T>(default, false, message, code);

        // ----- Implicit conversions -----

        public static implicit operator bool(Result<T> result) => result.IsSuccess;
        public static implicit operator T?(Result<T> result) => result.Value;
    }

    /// <summary>
    /// Provides generic helper methods for cleaner syntax.
    /// </summary>
    public static class ResultHelpers
    {
        /// <summary>
        /// Inferred Success() — allows "return Result.Success(value)" without specifying type.
        /// </summary>
        public static Result<T> Success<T>(T value) => Result<T>.Success(value);

        /// <summary>
        /// Combine multiple generic results (useful for multi-stage validation).
        /// </summary>
        public static Result Combine(params Result[] results) => Result.Combine(results);
    }

    /// <summary>
    /// Optional structured error codes for consistent client responses.
    /// </summary>
    public enum ErrorCode
    {
        ValidationError,
        NotFound,
        Duplicate,
        Unauthorized,
        Unexpected
    }
}
