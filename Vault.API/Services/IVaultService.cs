using Vault.API.Models;

namespace Vault.API.Services;

public interface IVaultService
{
    Task<GetApiKeyResponse> GetApiKey(string vendorName);
    Task<CreateApiKeyResponse> CreateApiKey(CreateApiKeyRequest request);
}
