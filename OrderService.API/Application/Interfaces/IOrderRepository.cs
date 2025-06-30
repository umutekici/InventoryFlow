using OrderService.API.Domain;

namespace OrderService.API.Application.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        List<Order> GetAll();
    }
}
