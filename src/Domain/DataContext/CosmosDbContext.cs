using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Domain.DataContext
{

    /// <summary>
    /// Azure Cosmos DB Context
    /// for setting up Entities and its relationships
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CosmosDbContext : DbContext
    {
        public CosmosDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.HasDefaultContainer("Product");

            modelBuilder.Entity<Product>()
                .HasNoDiscriminator()
                .HasPartitionKey(x => x.Category)
                .HasKey(x => x.ProductId);

            modelBuilder.Entity<Supplier>()
                .HasNoDiscriminator()
                .ToContainer("Suppliers")
                .HasPartitionKey(x => x.ProductId)
                .HasKey(x => x.SupplierId);

            modelBuilder.Entity<Inventory>()
                .HasNoDiscriminator()
                .ToContainer("Inventory")
                .HasPartitionKey(x => x.ProductId)
                .HasKey(x => x.InventoryId);
        }
    }
}
