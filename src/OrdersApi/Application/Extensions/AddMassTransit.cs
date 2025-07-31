using MassTransit;

namespace OrdersApi.Application.Extensions
{
    public static class AddMassTransit
    {
        /// <summary>
        /// Adds MassTransit configuration to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddMassTransitExtension(this WebApplicationBuilder? builder)
        {
            builder.Services.AddMassTransit(options =>
            {
                options.SetKebabCaseEndpointNameFormatter();

                options.UsingRabbitMq((context, cfg) =>
                {
                    var _isDockerEnabled = bool.TryParse(builder.Configuration["Docker:Enabled"]!, out bool result);

                    if(_isDockerEnabled)
                    {
                        cfg.Host(new Uri(builder.Configuration["RabbitMQ:Host"]!), host =>
                        {
                            host.Username(builder.Configuration["RabbitMQ:Username"]!);
                            host.Password(builder.Configuration["RabbitMQ:Password"]!);
                        });
                    }
                    else
                    {
                        var configuration = context.GetRequiredService<IConfiguration>();
                        var host = configuration.GetConnectionString("RabbitMQConnection");
                        cfg.Host(host);
                    }
                    cfg.ConfigureEndpoints(context);
                });
            });

            return builder.Services;
        }
    }
}
