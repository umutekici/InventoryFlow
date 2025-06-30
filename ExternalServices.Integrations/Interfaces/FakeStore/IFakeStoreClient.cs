using ExternalServices.Integrations.Models.FakeStore;

namespace ExternalServices.Integrations.Interfaces.FakeStore
{
    public interface IFakeStoreClient
    {
        Task<List<FakeStoreProduct>> GetProductsAsync();
    }
}
