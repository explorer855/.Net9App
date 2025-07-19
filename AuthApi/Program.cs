using AuthApi.Application.Middlewares;
using AuthApi.Application.Validators;
using AuthApi.Data;
using AuthApi.Infrastructure.Services;
using AuthApi.Models.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace AuthApi
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Exception Handling 
            builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            //Setup Fluent Validations
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);           

            // Add services to the container. Register the DbContext with Cosmos DB
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Identity with Cosmos DB
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.MaxDepth= 64; // Set maximum depth for JSON serialization
                    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Preserve dictionary keys as they are defined
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opts =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            

            var app = builder.Build();

            app.UseExceptionHandler(opts => { });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseMvc();
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            //ApplyMigrations(builder);

            app.Run();
        }

        public static void ApplyMigrations(WebApplicationBuilder webApplicationBuilder)
        {
            using (var scope = webApplicationBuilder.Services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
