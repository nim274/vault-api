using Vault.API.Repositories.Entities;

namespace Vault.API.Repositories
{
    public interface IVaultRepository
    {
        Task<ApiKey> CreateApiKey(ApiKey key);
        Task CreateApiKeyRequest(int keyId);
        Task<ApiKey?> GetApiKeyByVendorName(string vendorName);
        Task<long> GetKeyRequestCount(int keyId);
    }
}
