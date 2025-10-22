using Microsoft.EntityFrameworkCore;
using Portifolio.Models.Models;

namespace Portifolio.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<Portfolio> Portfolios => Set<Portfolio>();

        public DbSet<Position> Positions => Set<Position>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Portfolio>()
                        .HasMany(p => p.Positions)
                        .WithOne()
                        .HasForeignKey(pos => pos.PortfolioId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PriceHistory>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
