using Vault.API.Exceptions;
using Vault.API.Repositories;
using Vault.API.Repositories.Entities;

namespace Vault.API.Infrastructure
{
    public class VaultRepository : IVaultRepository
    {
        public async Task<ApiKey> GetApiKeyByVendorName(string vendorName)
        {
            var key = _keyTable.FirstOrDefault(x => x.VendorName.Equals(vendorName, StringComparison.OrdinalIgnoreCase));

            if (key == null)
                throw new ApiKeyNotFoundException(1, $"Api key not found for vendor:{vendorName}");

            await InsertKeyRequest(key.ApiKeyId);
            
            return key;
        }

        public async Task<long> GetKeyRequestCount(int keyId)
        {
            var count = _keyRequestTable.Count(x => x.ApiKeyId == keyId);
            return await Task.FromResult(count);
        }

        private async Task InsertKeyRequest(int keyId)
        {
            await Task.CompletedTask; // In theory current method is async, but no further async call in this example

            var newRequestId = _keyRequestTable.Count + 1;
            var keyRequest = new ApiKeyRequest() { ApiKeyRequestId = newRequestId, ApiKeyId = keyId, RequestDate = DateTime.Now };

            _keyRequestTable.Add(keyRequest);
        }

        public async Task<ApiKey> CreateApiKey(ApiKey key)
        {
            await Task.CompletedTask; // In theory current method is async, but no further async call in this example

            var existingKey = _keyTable.Where(x => x.VendorName.Equals(key.VendorName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (existingKey != null)
                throw new DuplicateApiKeyException(2, "Key for the vendor already exists");

            key.ApiKeyId = _keyTable.Count + 1;
            key.FirstAddedDate = DateTime.Now;
            _keyTable.Add(key);

            return key;
        }

        private static List<ApiKey> _keyTable = new List<ApiKey>()
        {
            new ApiKey() { ApiKeyId = 1, Value = "bJBsoBUROynduhQtk8CCoXwF9EKyzC4Y", VendorName = "Cobalt", FirstAddedDate = new DateTime(2024, 1, 1) },
            new ApiKey() { ApiKeyId = 2, Value = "mrv2yVhmGOstkEYnOzHWpwzpysHWVLRi", VendorName = "Lithium", FirstAddedDate = new DateTime(2024, 2, 1) },
            new ApiKey() { ApiKeyId = 3, Value = "wLlpAyQMdu1pyGC4MdeWiZvcSDbr64Ua", VendorName = "Gallium", FirstAddedDate = new DateTime(2024, 3, 1) },
        };


        private static List<ApiKeyRequest> _keyRequestTable = new List<ApiKeyRequest>()
        {
            new () { ApiKeyRequestId = 1, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 1) },
            new () { ApiKeyRequestId = 4, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 2) },
            new () { ApiKeyRequestId = 5, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 3) },
            new () { ApiKeyRequestId = 6, ApiKeyId = 1, RequestDate = new DateTime(2024, 1, 4) },

            new () { ApiKeyRequestId = 6, ApiKeyId = 2, RequestDate = new DateTime(2024, 1, 4) },
            new () { ApiKeyRequestId = 6, ApiKeyId = 2, RequestDate = new DateTime(2024, 1, 4) },
            new () { ApiKeyRequestId = 6, ApiKeyId = 2, RequestDate = new DateTime(2024, 1, 4) },

            new () { ApiKeyRequestId = 6, ApiKeyId = 3, RequestDate = new DateTime(2024, 1, 4) },
            new () { ApiKeyRequestId = 6, ApiKeyId = 4, RequestDate = new DateTime(2024, 1, 4) },
        };
    }
}
