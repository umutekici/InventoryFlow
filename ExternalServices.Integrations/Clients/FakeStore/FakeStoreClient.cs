using ExternalServices.Integrations.Interfaces.FakeStore;
using ExternalServices.Integrations.Models.FakeStore;
using ExternalServices.Integrations.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ExternalServices.Integrations.Clients.FakeStore
{
    public class FakeStoreClient : IFakeStoreClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _productsEndpoint;

        public FakeStoreClient(HttpClient httpClient, IOptions<FakeStoreOptions> options)
        {
            var baseUrl = options?.Value?.BaseUrl ?? throw new InvalidOperationException("FakeStore BaseUrl is not configured.");
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(baseUrl);
            _productsEndpoint = options?.Value?.Endpoint ?? throw new InvalidOperationException("ProductsEndpoint is not configured.");
        }

        public async Task<List<FakeStoreProduct>> GetProductsAsync()
        {
           
            var products = await _httpClient.GetFromJsonAsync<List<FakeStoreProduct>>(_productsEndpoint);
            return products ?? new List<FakeStoreProduct>();
        }
    }
}
