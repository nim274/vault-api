using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Vault.API.Exceptions;
using Vault.API.Models;
using Vault.API.Repositories;
using Vault.API.Repositories.Entities;
using Vault.API.Services;

namespace Vault.API.Tests.UnitTests
{
    public class VaultServiceTests
    {
        private readonly Mock<IVaultRepository> _vaultRepositoryMock;
        private readonly IVaultService _vaultService;

        public VaultServiceTests()
        {
            _vaultRepositoryMock = new Mock<IVaultRepository>();
            _vaultService = new VaultService(_vaultRepositoryMock.Object);
        }

        [Fact]
        public async Task GetApiKey_ApiKeyExistsInDatabase_ReturnsKey()
        {
            // Arrange
            const int apiKeyId = 1;
            const string vendorName = "Cobalt";
            const string keyValue = "345234uouoi";
            var firstAdded = DateTime.Now;

            var apiKey = new ApiKey() { ApiKeyId = apiKeyId, KeyValue = keyValue, VendorName = vendorName, FirstAddedDate = firstAdded };

            _vaultRepositoryMock.Setup(x => x.GetApiKeyByVendorName(vendorName)).ReturnsAsync(apiKey);
            _vaultRepositoryMock.Setup(x => x.GetKeyRequestCount(apiKeyId)).ReturnsAsync(4);

            // Act
            var response = await _vaultService.GetApiKey(vendorName);


            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.Key.Should().Be(keyValue);
                response.VendorName.Should().Be(vendorName);
                response.FirstAddedDate.Should().Be(firstAdded);
                response.RequestCount.Should().Be(4);
            }
        }

        [Fact]
        public async Task GetApiKey_ApiKeyDoesNotExistInDatabase_ThrowsApiKeyNotFoundException()
        {
            // Arrange
            const string vendorName = "Neon";
            var exception = new ApiKeyNotFoundException(1, "Api Key Not found");

            _vaultRepositoryMock.Setup(x => x.GetApiKeyByVendorName(vendorName)).ThrowsAsync(exception);

            // Act
            var act = async () => await _vaultService.GetApiKey(vendorName);


            // Assert
            using (new AssertionScope())
            {
                await act.Should().ThrowAsync<ApiKeyNotFoundException>();
            }
        }

        [Fact]
        public async Task CreateApiKey_ApiKeyDoesNotExistInDatabase_ReturnsNewlyCreatedApiKey()
        {
            // Arrange
            const string vendorName = "Neon";
            const string keyValue = "345234uouoi";
            var firstAdded = DateTime.Now;

            var serviceRequest = new CreateApiKeyRequest() { Key = keyValue, VendorName = vendorName };
            var repositoryResponse = new ApiKey() { ApiKeyId = 1, KeyValue = keyValue, VendorName = vendorName, FirstAddedDate = firstAdded };

            _vaultRepositoryMock.Setup(x => x.CreateApiKey(It.Is<ApiKey>(x => x.VendorName.Equals(vendorName) && x.KeyValue.Equals(keyValue)))).ReturnsAsync(repositoryResponse);

            // Act
            var response = await _vaultService.CreateApiKey(serviceRequest);


            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.Key.Should().Be(keyValue);
                response.VendorName.Should().Be(vendorName);
                response.FirstAddedDate.Should().Be(firstAdded);
            }
        }

        [Fact]
        public async Task CreateApiKey_ApiKeyExistsForVendor_ThrowsDuplicateApiKeyException()
        {
            // Arrange
            const string vendorName = "Cobalt";
            const string keyValue = "345234uouoi";
            var firstAdded = DateTime.Now;

            var apiKeyRequest = new CreateApiKeyRequest() { Key = keyValue, VendorName = vendorName };

            _vaultRepositoryMock.Setup(x => x.CreateApiKey(It.Is<ApiKey>(x => x.VendorName.Equals(vendorName) && x.KeyValue.Equals(keyValue))))
                .ThrowsAsync(new DuplicateApiKeyException(2, "Key already present"));

            // Act
            var act = async () => await _vaultService.CreateApiKey(apiKeyRequest);


            // Assert
            using (new AssertionScope())
            {
                await act.Should().ThrowAsync<DuplicateApiKeyException>();
            }
        }
    }
}
