using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services
{
    public class ProductService
        : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                product.CreatedOn = DateTime.UtcNow;
                return await _repository.Add(product);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            var product = await GetProductById(productId);
            return await _repository.Delete(productId, product);
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            return await _repository.GetById(productId);
        }

        public async Task<Product?> UpdateProduct(Guid productId, Product product)
        {
            product.ProductId = productId;
            product.UpdatedOn = DateTime.UtcNow;
            return await _repository.Update(productId, product);
        }
    }
}
