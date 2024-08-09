namespace Vault.API.Models
{
    public class GetApiKeyResponse : ApiKeyResponse
    {
        public long RequestCount { get; set; }
    }
}
