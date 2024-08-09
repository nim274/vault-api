namespace Vault.API.Exceptions
{
    public class ApiKeyNotFoundException : Exception
    {
        public ErrorDetail ErrorDetail { get; set; }
        public ApiKeyNotFoundException(int errorCode, string message)
        {
            ErrorDetail = new ErrorDetail(errorCode, message);
        }
    }
}
