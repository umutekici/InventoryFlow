using OrderService.API.Application.Interfaces;
using OrderService.API.Domain;

namespace OrderService.API.Infrastructure.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public void Add(Order order)
        {
            _orders.Add(order);
        }

        public List<Order> GetAll()
        {
            return _orders.ToList();
        }

    }
}
