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
            var key = new ApiKey
            {
                Value = request.Key,
                VendorName = request.VendorName
            };

            var createdKey = await _vaultRepository.CreateApiKey(key);
            
            return new CreateApiKeyResponse()
            {
                Key = createdKey.Value,
                FirstAddedDate = createdKey.FirstAddedDate,
                VendorName = createdKey.VendorName
            };
        }

        public async Task<ApiKeyResponse> GetApiKey(string vendorName)
        {
            var apiKey = await _vaultRepository.GetApiKeyByVendorName(vendorName);

            var count = await _vaultRepository.GetKeyRequestCount(apiKey.ApiKeyId);

            return new GetApiKeyResponse()
            {
                Key = apiKey.Value,
                FirstAddedDate = apiKey.FirstAddedDate,
                VendorName = apiKey.VendorName,
                RequestCount = count
            };
        }
    }
}
