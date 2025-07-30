var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.IdentityApi>("IdentityApi");

builder.AddProject<Projects.CatalogApi>("CatalogApi");

builder.AddProject<Projects.OrdersApi>("ordersapi");

builder.AddProject<Projects.OutboxBackgroundApp>("outboxbackgroundapp");

builder.Build().Run();
