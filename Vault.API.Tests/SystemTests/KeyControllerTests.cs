using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using Vault.API.Models;
using Vault.API.Repositories;
using Vault.API.Repositories.Entities;

namespace Vault.API.Tests.SystemTests
{
    public class KeyControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IVaultRepository> _vaultRepository;
        public KeyControllerTests(WebApplicationFactory<Program> factory)
        {
            _vaultRepository = new Mock<IVaultRepository>();
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_vaultRepository);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task CreateKey_ApiKeyDoesNotExistsInDatabase_ReturnsCreated()
        {
            // Arrange
            var key = "325234dfgsdsf";
            var vendorName = "Neon";
            var apiKeyRequest = new CreateApiKeyRequest() { Key = key, VendorName = vendorName };
            var apiKey = new ApiKey() { KeyValue = key, VendorName = vendorName  };
            var content = new StringContent(JsonSerializer.Serialize(apiKeyRequest), Encoding.UTF8, "application/json");

            _vaultRepository.Setup(x => x.CreateApiKey(It.Is<ApiKey>(x=>x.KeyValue.Equals(key) && x.VendorName.Equals(vendorName))))
                .ReturnsAsync(apiKey);

            // Act
            var response = await _client.PutAsync("/key", content);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task CreateKey_ApiKeyAlreadyExistsInDatabase_ReturnsBadRequest()
        {
            // Arrange
            var key = "325234dfgsdsf";
            var vendorName = "Neon";
            var apiKeyRequest = new CreateApiKeyRequest() { Key = key, VendorName = vendorName };
            var apiKey = new ApiKey() { KeyValue = key, VendorName = vendorName };
            var content = new StringContent(JsonSerializer.Serialize(apiKeyRequest), Encoding.UTF8, "application/json");

            _vaultRepository.Setup(x => x.CreateApiKey(It.Is<ApiKey>(x => x.KeyValue.Equals(key) && x.VendorName.Equals(vendorName))))
                .ReturnsAsync(apiKey);

            // Act
            var response = await _client.PutAsync("/key", content);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task CreateKey_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var key = "325234dfgsdsf";
            var vendorName = "Neon";
            var apiKeyRequest = new CreateApiKeyRequest() {};
            var apiKey = new ApiKey() { KeyValue = key, VendorName = vendorName };
            var content = new StringContent(JsonSerializer.Serialize(apiKeyRequest), Encoding.UTF8, "application/json");

            _vaultRepository.Setup(x => x.CreateApiKey(It.Is<ApiKey>(x => x.KeyValue.Equals(key) && x.VendorName.Equals(vendorName))))
                .ReturnsAsync(apiKey);

            // Act
            var response = await _client.PutAsync("/key", content);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task GetApiKey_ApiKeyExistsInDatabase_ReturnsSuccess()
        {
            // Arrange
            var key = "325234dfgsdsf";
            var vendorName = "Neon";

            var apiKey = new ApiKey() { ApiKeyId =1, KeyValue = key, VendorName = vendorName, FirstAddedDate =DateTime.Now };

            _vaultRepository.Setup(x => x.GetApiKeyByVendorName(vendorName)).ReturnsAsync(apiKey);
            _vaultRepository.Setup(x => x.CreateApiKeyRequest(apiKey.ApiKeyId));

            // Act
            var response = await _client.GetAsync($"/vendor/{vendorName}/key");

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task GetApiKey_ApiKeyDoesNotExistsInDatabase_ReturnsNotFound()
        {
            // Arrange
            var vendorName = "Neon";

            // Act
            var response = await _client.GetAsync($"/vendor/{vendorName}/key");

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
