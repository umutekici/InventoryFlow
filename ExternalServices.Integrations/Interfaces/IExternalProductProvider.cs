using ExternalServices.Integrations.Models.FakeStore;

namespace ExternalServices.Integrations.Interfaces
{
    public interface IExternalProductProvider
    {
        Task<List<FakeStoreProduct>> GetProductsAsync();
    }
}
