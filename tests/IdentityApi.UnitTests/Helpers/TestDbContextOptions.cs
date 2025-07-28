using IdentityApi.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.Unit.Tests.Helpers
{
    internal class TestDbContextOptions
    {
        public static DbContextOptions<AuthDbContext> GetOptions()
        {
            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return options;
        }
    }
}
