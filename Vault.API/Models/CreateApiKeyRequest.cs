using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vault.API.Models;

public class CreateApiKeyRequest
{
    [Required]
    [DisplayName("Key")]
    public string Key { get; set; }
    [Required]
    [DisplayName("VendorName")]
    public string VendorName { get; set; }
}
