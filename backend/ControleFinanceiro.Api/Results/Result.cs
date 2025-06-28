namespace ControleFinanceiro.Api.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            if (isSuccess && error != null)
            {
                throw new ArgumentException("Error tem que ser nulo quando o valor de IsSuccess � true.");
            }
            if (!isSuccess && error == null)
            {
                throw new ArgumentException("Error n�o pode ser nulo quando o valor de IsSuccess � false.");
            }
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
    }
}