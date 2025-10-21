using Microsoft.EntityFrameworkCore;
using Portifolio.Models.Models;

namespace Portifolio.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<Portfolio> Portfolios => Set<Portfolio>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
