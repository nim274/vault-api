using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vault.API.Models
{
    public class CreateApiKeyRequest
    {
        [Required]
        [DisplayName("Api Key")]
        public string Key { get; set; }
        [Required]
        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }
    }
}
