using Microsoft.AspNetCore.Mvc;
using OrderService.API.Application.Interfaces;

namespace OrderService.API.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Retrieves all orders from the system.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        
        [HttpGet("order-list")]
        public IActionResult GetAllOrders()
        {
            var orders = _orderRepository.GetAll();
            return Ok(orders);
        }
    }
}
