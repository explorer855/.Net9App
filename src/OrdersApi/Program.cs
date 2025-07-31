using MassTransit;
using OrdersApi.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

bool isDockerEnabled = bool.TryParse(builder.Configuration["Docker:Enabled"], out bool result);

if (!isDockerEnabled)
{
    builder.AddServiceDefaults();
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Plugin MassTransit

builder.AddMassTransitExtension();

#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
