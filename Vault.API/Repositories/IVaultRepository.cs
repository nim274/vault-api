using Vault.API.Repositories.Entities;

namespace Vault.API.Repositories
{
    public interface IVaultRepository
    {
        Task<ApiKey> CreateApiKey(ApiKey key);
        Task<ApiKey> GetApiKeyByVendorName(string vendorName);
        Task<long> GetKeyRequestCount(int keyId);
    }
}
