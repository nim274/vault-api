namespace Vault.API.Repositories.Entities;

public class ApiKey
{
    public int ApiKeyId { get; set; }
    public string KeyValue { get; set; }
    public string VendorName { get; set; }
    public DateTime FirstAddedDate { get; set; }
}
