using AuthApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Unit.Tests.Helpers
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
