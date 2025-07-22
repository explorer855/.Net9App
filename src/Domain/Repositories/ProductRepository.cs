using Domain.DataContext;
using Domain.Entities;

namespace Domain.Repositories
{
    public class ProductRepository :
        Repository<Product>, IProductRepository
    {
        public ProductRepository(CosmosDbContext dbContext)
            : base(dbContext) { }

        public async Task<Product?> Add(Product product)
        {
            try
            {
                await AddEntry(product);

                return product;
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<bool> Delete(Guid productId, Product product)
        {
            return await DeleteEntry(product);
        }

        public async Task<Product?> GetById(Guid productId)
        {
            return await LoadProductWithReferences(productId);
        }

        public async Task<Product?> Update(Guid productId, Product product)
        {
            var existingProduct = await LoadProductWithReferences(productId);

            if (existingProduct == null) return null;

            existingProduct.Name = product.Name;
            existingProduct.Category = product.Category;
            existingProduct.Dimensions = product.Dimensions;
            existingProduct.ShippingOptions = product.ShippingOptions;
            existingProduct.Suppliers = product.Suppliers;
            existingProduct.Inventory = product.Inventory;
            existingProduct.UpdatedOn = product.UpdatedOn;

            await UpdateEntry(existingProduct);
            return existingProduct;
        }

        private async Task<Product?> LoadProductWithReferences(Guid productId)
        {
            var product = await _context
                .Products
                .FindAsync(productId);
            if (product == null) return null;

            var productEntry = _context.Products.Entry(product);

            // Include the Inventory (which comes from another container)
            await productEntry
                .Reference(product => product.Inventory)
                .LoadAsync();

            // Include the Suppliers (which come from another container)
            await productEntry
                .Collection(product => product.Suppliers)
                .LoadAsync();

            return product;
        }
    }
}
