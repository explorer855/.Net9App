using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApi.Models.Entities;

namespace WebApi.Data.DataContext
{

    /// <summary>
    /// Azure Cosmos DB Context
    /// for setting up Entities and its relationships
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CatalogDbContext : DbContext
    {
        public DbSet<CatalogItem> Catalogs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Default Container
            modelBuilder.HasDefaultContainer("CatalogItems");
            #endregion

            #region Catalog Container
            modelBuilder.Entity<CatalogItem>()
                .HasNoDiscriminator().
                ToContainer("CatalogItem")
                .HasKey(x => x.Id);
            #endregion

            #region ETag
            modelBuilder.Entity<CatalogItem>()
                .UseETagConcurrency();
            #endregion

            #region Configure Navigations

            modelBuilder.Entity<CatalogItem>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CatalogItem>()
                .OwnsOne(x =>x.CatalogBrand);

            modelBuilder.Entity<CatalogItem>()
                .OwnsOne(x => x.CatalogType);

            #endregion

            modelBuilder.HasAutoscaleThroughput(1000);

            base.OnModelCreating(modelBuilder);
        }
    }
}
