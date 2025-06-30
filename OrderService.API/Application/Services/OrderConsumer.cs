using ExternalServices.Integrations.Enums;
using ExternalServices.Integrations.Factories;
using ExternalServices.Integrations.Helpers;
using MassTransit;
using Messaging.Common.Interfaces;
using OrderService.API.Application.Interfaces;

namespace OrderService.API.Application.Services
{
    public class OrderConsumer : IConsumer<ICreateOrder>
    {
        private readonly ExternalProductProviderFactory _factory;
        private readonly IOrderRepository _orderRepository;
        private readonly OrderCreationService _orderCreationService;

        public OrderConsumer(ExternalProductProviderFactory factory, IOrderRepository orderRepository, OrderCreationService orderCreationService)
        {
            _factory = factory;
            _orderRepository = orderRepository;
            _orderCreationService = orderCreationService;
        }

        public async Task Consume(ConsumeContext<ICreateOrder> context)
        {

            var message = context.Message;

            var adapter = _factory.Create(ExternalProviders.FakeStore);
            var products = await adapter.GetProductsAsync();

            var cheapestProduct = FakeStoreProductSelector.GetCheapestProductByCode(products, message.ProductCode);

            if (cheapestProduct == null)
                return;

            var order = _orderCreationService.CreateOrder(message, cheapestProduct.Price);

            _orderRepository.Add(order);
        }
    }
}
