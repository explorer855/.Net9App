var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.IdentityApi>("IdentityApi");

builder.AddProject<Projects.CatalogApi>("CatalogApi");

builder.Build().Run();
