using Infrastructure.CqrsCommon;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Dispathchers
{
    [ExcludeFromCodeCoverage]
    internal class QueryDispathcer(IServiceProvider serviceProvider)
        : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation)
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();

            return handler.HandleAsync(query, cancellation);
        }
    }
}
