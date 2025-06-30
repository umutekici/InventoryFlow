using Messaging.Common.Interfaces;
using OrderService.API.Domain;
using OrderService.API.Enums;

namespace OrderService.API.Application.Services
{
    public class OrderCreationService
    {
        public Order CreateOrder(ICreateOrder message, decimal unitPrice)
        {
            return new Order
            {
                Id = message.OrderId,
                ProductCode = message.ProductCode,
                Quantity = message.Quantity,
                UnitPrice = unitPrice,
                OrderDate = DateTime.UtcNow,
                Status = OrderTypes.Created
            };
        }
    }
}
