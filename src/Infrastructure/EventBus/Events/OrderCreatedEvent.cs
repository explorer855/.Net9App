using Infrastructure.EventBus.Events.Interfaces;

namespace Infrastructure.EventBus.Events;

internal class OrderCreatedEvent
    : IOrderCreatedEvent
{
    public List<OrderItem>? OrderItemList { get; set; }

    public Guid CorrelationId { get; set; }
}
