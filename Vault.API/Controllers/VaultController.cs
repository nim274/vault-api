using Microsoft.AspNetCore.Mvc;
using Vault.API.Models;
using Vault.API.Services;

namespace Vault.API.Controllers
{
    [ApiController]
    [Route("key")]
    public class VaultController : ControllerBase
    {
        private readonly IVaultService _vaultService;
        public VaultController(IVaultService vaultService)
        {
            _vaultService = vaultService;
        }

        [HttpGet("{vendorName}")]
        public async Task<IActionResult> GetByVendorName(string vendorName)
        {
            var response = await _vaultService.GetApiKey(vendorName);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> CreateKey(CreateApiKeyRequest request)
        {
            var response = await _vaultService.CreateApiKey(request);

            return Created($"{Request.Path.Value}/{request.VendorName}", response);
        }
    }
}
