using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> Add(Product product);
        Task<Product?> GetById(Guid productId);
        Task<Product?> Update(Guid productId, Product product);
        Task<bool> Delete(Guid productId, Product product);
    }
}
