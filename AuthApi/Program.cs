
using AspNetCore.Identity.CosmosDb.Extensions;
using AuthApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace AuthApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Setup Cosmos DB

            //SeedCosmosDb(builder);

            // Add services to the container. Register the DbContext with Cosmos DB
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Identity with Cosmos DB
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddDefaultUI() // Use this if Identity Scaffolding is in use
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseMvc();
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            ApplyMigrations(builder);

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
