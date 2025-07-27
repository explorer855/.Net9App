using WebApi.Models.Entities;

namespace WebApi.Application.Repositories;

public interface ICatalogRepository
{
    Task<CatalogItem?> Add(CatalogItem Catalog);
    Task<CatalogItem?> GetById(Guid CatalogId);
    Task<IReadOnlyList<string>?> GetCatalogCategories();
    Task<CatalogItem?> Update(Guid CatalogId, CatalogItem Catalog);
    Task<bool> Delete(Guid CatalogId, CatalogItem Catalog);
}
