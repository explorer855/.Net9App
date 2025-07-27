using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.DataContext;
using WebApi.Models.Entities;

namespace WebApi.Application.Repositories;

public class CatalogRepository :
    Repository<CatalogItem>, ICatalogRepository
{
    public CatalogRepository(CatalogDbContext dbContext)
        : base(dbContext)
    {

    }

    public async Task<CatalogItem?> Add(CatalogItem Catalog)
    {
        try
        {
            await AddEntry(Catalog);

            return Catalog;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> Delete(Guid CatalogId, CatalogItem Catalog)
    {
        return await DeleteEntry(Catalog);
    }

    public async Task<CatalogItem?> GetById(Guid CatalogId)
    {
        return await _dbCtx.Catalogs.FindAsync(CatalogId);
    }

    public async Task<IReadOnlyList<string>?> GetCatalogCategories()
    {
        return await _dbCtx.Catalogs
            .Select(x => x.CatalogType.Type)
            .ToListAsync();
    }

    public async Task<CatalogItem?> Update(Guid CatalogId, CatalogItem Catalog)
    {
        var existingCatalog = await LoadCatalogEntity(CatalogId);

        if (existingCatalog == null) return null;

        await UpdateEntry(existingCatalog);
        return existingCatalog;
    }

    private async Task<CatalogItem?> LoadCatalogEntity(Guid CatalogId)
    {
        var Catalog = await _dbCtx
            .Catalogs
            .FindAsync(CatalogId);
        if (Catalog == null) return null;

        var CatalogEntry = _dbCtx.Catalogs.Entry(Catalog);

        return Catalog;
    }
}
