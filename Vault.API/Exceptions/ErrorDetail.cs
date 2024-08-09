namespace Vault.API.Exceptions
{
    public class ErrorDetail
    {
        public int ErrorId { get; set; }
        public string Message { get; set; }

        public ErrorDetail(int errorId, string message)
        {
            ErrorId = errorId;
            Message = message;
        }
    }
}
