using OrderService.API.Enums;

namespace OrderService.API.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderTypes? Status { get; set; }
    }
}
