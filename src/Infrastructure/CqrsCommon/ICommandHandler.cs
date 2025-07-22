namespace Infrastructure.CqrsCommon
{
    internal interface ICommandHandler<in TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken token = default);
    }
}
