using MassTransit;

namespace Infrastructure.EventBus.Events.Interfaces
{
    internal interface IOrderCreatedEvent
        : CorrelatedBy<Guid>
    {
        List<OrderItem> OrderItemList { get; set; }
    }
}
