using ExternalServices.Integrations.Adapters.FakeStore;
using ExternalServices.Integrations.Enums;
using ExternalServices.Integrations.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.Integrations.Factories
{
    public class ExternalProductProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExternalProductProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public IExternalProductProvider Create(ExternalProviders provider)
        {
            return provider switch
            {
                ExternalProviders.FakeStore => _serviceProvider.GetRequiredService<FakeStoreAdapter>(),
                _ => throw new ArgumentException($"Unknown provider: {provider}")
            };
        }
    }
}
