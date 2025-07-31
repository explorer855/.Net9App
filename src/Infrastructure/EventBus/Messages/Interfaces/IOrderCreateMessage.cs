using Infrastructure.EventBus.Events;

namespace Infrastructure.EventBus.Messages.Interfaces;

/// <summary>
/// MassTransit message interface for order creation events.
/// </summary>
public interface IOrderCreateMessage
{
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
    public string PaymentAccountId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> OrderItemList { get; set; }
}
