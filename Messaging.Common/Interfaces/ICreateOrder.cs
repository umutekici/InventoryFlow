namespace Messaging.Common.Interfaces
{
    public interface ICreateOrder
    {
        Guid OrderId { get; }
        int ProductCode { get; }
        int Quantity { get; }
        DateTime Timestamp { get; }
    }
}
