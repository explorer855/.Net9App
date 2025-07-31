using OrdersApi.Application.Extensions;
using OutboxBackgroundApp;

var builder = Host.CreateApplicationBuilder(args);

//#region Enable Area When .NET Aspire Container Orchestration is in use

//bool isDockerEnabled = bool.TryParse(builder.Configuration["Docker:Enabled"], out bool result);

//if (!isDockerEnabled)
//{
//    builder.AddServiceDefaults();
//}

//#endregion

builder.Services.AddHostedService<Worker>();
builder.AddMassTransitExtension();

var host = builder.Build();
host.Run();
