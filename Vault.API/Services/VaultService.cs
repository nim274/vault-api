using Vault.API.Exceptions;
using Vault.API.Models;
using Vault.API.Repositories;
using Vault.API.Repositories.Entities;

namespace Vault.API.Services
{
    public class VaultService : IVaultService
    {
        private readonly IVaultRepository _vaultRepository;
        public VaultService(IVaultRepository vaultRepository)
        {
            _vaultRepository = vaultRepository;
        }

        public async Task<CreateApiKeyResponse> CreateApiKey(CreateApiKeyRequest request)
        {
            var existingKey = await _vaultRepository.GetApiKeyByVendorName(request.VendorName);
            if (existingKey != null)
                throw new DuplicateApiKeyException(2, "Key for the vendor already exists");

            var key = new ApiKey
            {
                KeyValue = request.Key,
                VendorName = request.VendorName
            };

            var createdKey = await _vaultRepository.CreateApiKey(key);
            
            return new CreateApiKeyResponse()
            {
                Key = createdKey.KeyValue,
                FirstAddedDate = createdKey.FirstAddedDate,
                VendorName = createdKey.VendorName
            };
        }

        public async Task<GetApiKeyResponse> GetApiKey(string vendorName)
        {
            var apiKey = await _vaultRepository.GetApiKeyByVendorName(vendorName);

            if (apiKey == null)
                throw new ApiKeyNotFoundException(1, $"Api key not found for vendor:{vendorName}");

            await _vaultRepository.CreateApiKeyRequest(apiKey.ApiKeyId);

            var count = await _vaultRepository.GetKeyRequestCount(apiKey.ApiKeyId);

            return new GetApiKeyResponse()
            {
                Key = apiKey.KeyValue,
                FirstAddedDate = apiKey.FirstAddedDate,
                VendorName = apiKey.VendorName,
                RequestCount = count
            };
        }
    }
}
