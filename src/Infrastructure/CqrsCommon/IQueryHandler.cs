namespace Infrastructure.CqrsCommon
{
    internal interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken token = default);
    }
}
