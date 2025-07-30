namespace Auction.Management.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IEnumerable<Error>? Errors { get; }
        public T? Data { get; }

        private Result(bool IsSuccess, IEnumerable<Error> Errors)
        {
            this.IsSuccess = IsSuccess;
            this.Errors = Errors;
        }
        private Result(bool IsSuccess, T? Data)
        {
            this.IsSuccess = IsSuccess;
            this.Data = Data;
        }

        public static Result<T> Success(T? Data) => new(true, Data);
        public static Result<T> Failure(Error Error) => new(false, [Error]);
        public static Result<T> Failure(IEnumerable<Error> Errors) => new(false, Errors);
    }

}
