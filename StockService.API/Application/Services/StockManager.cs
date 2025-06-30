using MassTransit;
using Messaging.Common.Interfaces;
using StockService.API.Application.Dtos;

namespace StockService.API.Application.Services
{
    public class StockManager
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public StockManager(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task CheckAndRequestOrderAsync(List<LowStockProductDto> products)
        {
            foreach (var product in products)
            {
                await _publishEndpoint.Publish<ICreateOrder>(new
                {
                    OrderId = Guid.NewGuid(),
                    ProductCode = product.Code,
                    Quantity = product.CriticalStock - product.Stock,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}