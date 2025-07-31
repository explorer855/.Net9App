using Infrastructure.EventBus.Events;
using MassTransit;

namespace Infrastructure.EventBus.Messages.Interfaces;

/// <summary>
/// MassTransit message interface for payment completion events.
/// </summary>
internal interface IPaymentCompleteMessage
    : CorrelatedBy<Guid>
{
    public Guid? CustomerId { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<OrderItem> OrderItemList { get; set; }
}
