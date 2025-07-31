namespace Infrastructure.EventBus.Events
{
    public record OrderItem(string ProductId,
                            int Quantity);
}
