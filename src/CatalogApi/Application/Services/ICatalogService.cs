using WebApi.Models.Entities;

namespace WebApi.Application.Services;

public interface ICatalogService
{
    Task<CatalogItem> AddCatalog(CatalogItem Catalog);
    Task<CatalogItem?> GetCatalogById(Guid CatalogId);
    Task<IReadOnlyList<string>?> GetCatalogCategories();
    Task<CatalogItem?> UpdateCatalog(Guid CatalogId, CatalogItem Catalog);
    Task<bool> DeleteCatalog(Guid CatalogId);
}
