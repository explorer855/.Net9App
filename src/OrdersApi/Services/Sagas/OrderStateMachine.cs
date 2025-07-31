using MassTransit;

namespace OrdersApi.Services.Sagas
{
    public class OrderStateMachine
        : MassTransitStateMachine<OrderState>
    {
        private readonly ILogger<OrderStateMachine> _logger;

        public OrderStateMachine()
        {
            
        }
    }

    public class OrderState: SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }
        public Guid? OrderId { get; set; }
        public string? CustomerId { get; set; }
        public string? PaymentAccountId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
