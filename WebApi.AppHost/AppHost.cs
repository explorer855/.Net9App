var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.IdentityApi>("IdentityApi");

builder.AddProject<Projects.CatalogApi>("CatalogApi");


// Uncomment the following lines to enable RabbitMQ and Outbox Background App

//var rabbitMqHost = builder.AddRabbitMQ("RabbitMQConnection")
//    .WithManagementPlugin();

//builder.AddProject<Projects.OrdersApi>("ordersapi")
//    .WithReference(rabbitMqHost);

//builder.AddProject<Projects.OutboxBackgroundApp>("outboxbackgroundapp")
//    .WithReference(rabbitMqHost);

builder.Build().Run();
