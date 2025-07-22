using Domain.DataContext;
using Domain.Entities;

namespace Domain.Repositories
{
    public abstract class Repository<TEntity>
        where TEntity : Entity
    {
        public readonly CosmosDbContext _context;

        protected Repository(CosmosDbContext context)
        {
            _context = context;
        }

        public async Task<Entity> AddEntry<Entity>(Entity entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await SaveChangesAsync();

                return entity;
            }
            catch
            {
                throw;
            }           
        }
        public async Task<Entity> UpdateEntry<Entity>(Entity entity)
        {
            try
            {
                _context.Update(entity);
                await SaveChangesAsync();

                return entity;
            }
            catch
            {
                throw;
            }
            
        }
        public async Task<bool> DeleteEntry(Entity entity)
        {
            try
            {
                _context.Remove(entity);
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
            await _context.SaveChangesAsync();
        }
    }
}
