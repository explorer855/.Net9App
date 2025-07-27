using WebApi.Application.Repositories;
using WebApi.Models.Entities;

namespace WebApi.Application.Services;

public class CatalogService
    : ICatalogService
{
    private readonly ICatalogRepository _repository;
    public CatalogService(ICatalogRepository repository)
    {
        _repository = repository;
    }
    public async Task<CatalogItem> AddCatalog(CatalogItem Catalog)
    {
        try
        {   
            return await _repository.Add(Catalog);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteCatalog(Guid CatalogId)
    {
        var Catalog = await GetCatalogById(CatalogId);
        return await _repository.Delete(CatalogId, Catalog);
    }

    public async Task<CatalogItem?> GetCatalogById(Guid CatalogId)
    {
        return await _repository.GetById(CatalogId);
    }
    public async Task<IReadOnlyList<string>?> GetCatalogCategories()
    {
        return await _repository.GetCatalogCategories();
    }

    public async Task<CatalogItem?> UpdateCatalog(Guid CatalogId, CatalogItem Catalog)
    {
        return await _repository.Update(CatalogId, Catalog);
    }
}
