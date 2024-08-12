namespace Vault.API.Exceptions
{
    public class DuplicateApiKeyException : Exception
    {
        public ErrorDetail ErrorDetail { get; set; }
        public DuplicateApiKeyException(int errorCode, string message)
        {
            ErrorDetail = new ErrorDetail(errorCode, message);
        }
    }
}
