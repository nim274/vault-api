using Vault.API.Repositories;
using Vault.API.Repositories.Entities;

namespace Vault.API.Infrastructure;

public class VaultRepository : IVaultRepository
{
    public async Task<ApiKey?> GetApiKeyByVendorName(string vendorName)
    {
        var apiKey = _keyTable.FirstOrDefault(x => x.VendorName.Equals(vendorName, StringComparison.OrdinalIgnoreCase));

        return await Task.FromResult(apiKey);
    }

    public async Task<long> GetKeyRequestCount(int keyId)
    {
        var count = _keyRequestTable.Count(x => x.ApiKeyId == keyId);
        return await Task.FromResult(count);
    }

    public async Task CreateApiKeyRequest(int keyId)
    {
        await Task.CompletedTask; // In theory current method has an async call, but not in this example

        var newRequestId = _keyRequestTable.Count + 1;
        var keyRequest = new ApiKeyRequest() { ApiKeyRequestId = newRequestId, ApiKeyId = keyId, RequestDate = DateTime.Now };

        _keyRequestTable.Add(keyRequest);
    }

    public async Task<ApiKey> CreateApiKey(ApiKey key)
    {
        var newId = _keyTable.Count + 1;
        key.ApiKeyId = newId;
        key.FirstAddedDate = DateTime.Now;
        _keyTable.Add(key);

        return await Task.FromResult(key);
    }

    private static List<ApiKey> _keyTable = new List<ApiKey>()
    {
        new ApiKey() { ApiKeyId = 1, KeyValue = "bJBsoBUROynduhQtk8CCoXwF9EKyzC4Y", VendorName = "Cobalt", FirstAddedDate = new DateTime(2024, 1, 1) },
        new ApiKey() { ApiKeyId = 2, KeyValue = "mrv2yVhmGOstkEYnOzHWpwzpysHWVLRi", VendorName = "Lithium", FirstAddedDate = new DateTime(2024, 2, 1) },
        new ApiKey() { ApiKeyId = 3, KeyValue = "wLlpAyQMdu1pyGC4MdeWiZvcSDbr64Ua", VendorName = "Gallium", FirstAddedDate = new DateTime(2024, 3, 1) },
    };

    private static List<ApiKeyRequest> _keyRequestTable = new List<ApiKeyRequest>()
    {
        new () { ApiKeyRequestId = 1, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 1) },
        new () { ApiKeyRequestId = 2, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 2) },
        new () { ApiKeyRequestId = 3, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 3) },
        new () { ApiKeyRequestId = 4, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 4) },

        new () { ApiKeyRequestId = 5, ApiKeyId = 2, RequestDate = new DateTime(2024, 1, 4) },
        new () { ApiKeyRequestId = 6, ApiKeyId = 2, RequestDate = new DateTime(2024, 1, 4) },
        new () { ApiKeyRequestId = 7, ApiKeyId = 2, RequestDate = new DateTime(2024, 1, 4) },

        new () { ApiKeyRequestId = 8, ApiKeyId = 3, RequestDate = new DateTime(2024, 1, 4) },
        new () { ApiKeyRequestId = 9, ApiKeyId = 3, RequestDate = new DateTime(2024, 1, 4) },
    };
}
