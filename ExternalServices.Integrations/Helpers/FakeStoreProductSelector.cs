using ExternalServices.Integrations.Models.FakeStore;

namespace ExternalServices.Integrations.Helpers
{
    public class FakeStoreProductSelector
    {
        public static FakeStoreProduct? GetCheapestProductByCode(List<FakeStoreProduct> products, int productCode)
        {
            return products
                .Where(p => p.Id == productCode)
                .OrderBy(p => p.Price)
                .FirstOrDefault();
        }
    }
}
