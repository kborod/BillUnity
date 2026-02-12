namespace Kborod.Utils
{
    public record Result<T>()
    {
        public bool IsSuccess;
        public T Value = default;
        public string Error = null;

        public static Result<T> Ok(T value) => new() { IsSuccess = true, Value = value };
        public static Result<T> Fail(string error) => new() { IsSuccess = false, Error = error };
    }
}