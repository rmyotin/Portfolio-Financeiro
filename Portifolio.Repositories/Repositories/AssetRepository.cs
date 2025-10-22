using Portifolio.Infrastructure.Context;
using Portifolio.Models.Models;
using Portifolio.Repositories.Interfaces;

namespace Portifolio.Repositories.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Asset> GetAll() => _context.Assets.ToList();

        public Asset? GetById(int id) => _context.Assets.Find(id);
        public Asset? GetBySymbol(string symbol)
        {
            return _context.Assets.FirstOrDefault(a => a.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(Asset asset)
        {
            _context.Assets.Add(asset);
            _context.SaveChanges();
        }

        public void Update(Asset asset)
        {
            _context.Assets.Update(asset);
            _context.SaveChanges();
        }

        public void Delete(string symbol)
        {
            var existing = GetBySymbol(symbol);
            if (existing != null)
            {
                _context.Assets.Remove(existing);
                _context.SaveChanges();
            }
        }

        public bool Exists(int id) => _context.Assets.Any(a => a.Id == id);

        public bool ExistsBySymbol(string symbol) => _context.Assets.Any(a => a.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));

        public List<PriceHistory>? GetPriceHistory(string symbol)
        {
            var asset = _context.Assets.FirstOrDefault(a => a.Symbol == symbol);
            return asset?.PriceHistory;
        }
    }
}
