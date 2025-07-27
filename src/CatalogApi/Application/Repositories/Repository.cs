using WebApi.Data.DataContext;
using WebApi.Models.Entities;

namespace Domain.Repositories
{
    public abstract class Repository<TEntity>
        where TEntity : Entity
    {
        protected readonly CatalogDbContext _dbCtx;

        protected Repository(CatalogDbContext context)
        {
            _dbCtx = context;
        }

        public async Task<TEntity> AddEntry(TEntity entity)
        {
            try
            {
                _dbCtx.Set<TEntity>().Add(entity);
                await SaveChangesAsync();

                return entity;
            }
            catch
            {
                throw;
            }           
        }
        public async Task<TEntity> UpdateEntry(TEntity entity)
        {
            try
            {
                _dbCtx.Update(entity);
                await SaveChangesAsync();

                return entity;
            }
            catch
            {
                throw;
            }
            
        }
        public async Task<bool> DeleteEntry(TEntity entity)
        {
            try
            {
                _dbCtx.Remove(entity);
                await SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
            
        }
        private async Task SaveChangesAsync()
        {
            await _dbCtx.SaveChangesAsync();
        }
    }
}
