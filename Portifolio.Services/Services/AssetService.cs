using Portifolio.Models.Models;
using Portifolio.Repositories.Repositories;

namespace Portifolio.Services.Services
{
    public class AssetService
    {
        private readonly AssetRepository _repository;

        public AssetService(AssetRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Asset> GetAll() => _repository.GetAll();

        public Asset? GetBySymbol(string symbol) => _repository.GetBySymbol(symbol);

        public bool Create(Asset asset)
        {
            if (_repository.Exists(asset.Symbol))
                return false;

            _repository.Add(asset);
            return true;
        }

        public bool Update(string symbol, Asset updatedAsset)
        {
            var existing = _repository.GetBySymbol(symbol);
            if (existing == null) return false;

            existing.Name = updatedAsset.Name;
            existing.Type = updatedAsset.Type;
            existing.Sector = updatedAsset.Sector;
            existing.CurrentPrice = updatedAsset.CurrentPrice;
            existing.LastUpdated = DateTime.UtcNow;

            _repository.Update(existing);
            return true;
        }

        public bool Delete(string symbol)
        {
            var existing = _repository.GetBySymbol(symbol);
            if (existing == null) return false;

            _repository.Delete(symbol);
            return true;
        }
    }
}
