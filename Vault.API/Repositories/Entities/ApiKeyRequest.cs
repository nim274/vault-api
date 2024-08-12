namespace Vault.API.Repositories.Entities;

public class ApiKeyRequest
{
    public int ApiKeyRequestId { get; set; }
    public int ApiKeyId { get; set; }

    public DateTime RequestDate { get; set; }
}
