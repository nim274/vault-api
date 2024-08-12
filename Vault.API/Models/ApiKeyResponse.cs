namespace Vault.API.Models;

public class ApiKeyResponse
{
    public string Key { get; set; }
    public string VendorName { get; set; }
    public DateTime FirstAddedDate { get; set; }
}
