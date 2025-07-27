using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApi.Application.Repositories;
using WebApi.Application.Services;
using WebApi.Data.DataContext;
using WebApi.Infrastructure.CultureMiddleware;
using WebApi.Infrastructure.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContextFactory<CatalogDbContext>(optionsBuilder =>
  optionsBuilder
    .UseCosmos(
      connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
      databaseName: "Catalog-store",
      cosmosOptionsAction: options =>
      {
          options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
          options.MaxRequestsPerTcpConnection(16);
          options.MaxTcpConnectionsPerEndpoint(32);
      }));

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.MaxDepth = 64; // Set maximum depth for JSON serialization
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Preserve dictionary keys as they are defined
        opts.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<CatalogDbContext>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(opts => { });

app.UseHttpsRedirection();
app.UseMiddleware<RequestCultureMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

