namespace ControleFinanceiro.Api.Results
{
    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null!);
        public static new Result<T> Failure(string error) => new Result<T>(default!, false, error);
    }
}
