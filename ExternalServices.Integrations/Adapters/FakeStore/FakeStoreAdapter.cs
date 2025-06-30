using ExternalServices.Integrations.Interfaces;
using ExternalServices.Integrations.Interfaces.FakeStore;
using ExternalServices.Integrations.Models.FakeStore;

namespace ExternalServices.Integrations.Adapters.FakeStore
{
    public class FakeStoreAdapter : IExternalProductProvider
    {
        private readonly IFakeStoreClient _fakeStoreClient;

        public FakeStoreAdapter(IFakeStoreClient fakeStoreClient)
        {
            _fakeStoreClient = fakeStoreClient;
        }

        public async Task<List<FakeStoreProduct>> GetProductsAsync()
        {
            return await _fakeStoreClient.GetProductsAsync();
        }
    }
}

