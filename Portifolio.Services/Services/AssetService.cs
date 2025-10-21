using Portifolio.Models.Models;
using Portifolio.Repositories.Interfaces;
using Portifolio.Services.Interfaces;

namespace Portifolio.Services.Services
{

    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _repository;

        public AssetService(IAssetRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Asset> GetAll() => _repository.GetAll();

        public Asset? GetById(int id) => _repository.GetById(id);

        public Asset? GetBySymbol(string symbol) => _repository.GetBySymbol(symbol);

        public (bool success, string message) Create(Asset asset)
        {
            if (_repository.ExistsBySymbol(asset.Symbol))
                return (false, $"O ativo com símbolo '{asset.Symbol}' já existe.");

            asset.LastUpdated = DateTime.UtcNow;
            _repository.Add(asset);
            return (true, "Ativo criado com sucesso.");
        }

        public (bool success, string message) Update(int id, Asset updatedAsset)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                return (false, "Ativo não encontrado.");

            existing.Symbol = updatedAsset.Symbol;
            existing.Name = updatedAsset.Name;
            existing.Type = updatedAsset.Type;
            existing.Sector = updatedAsset.Sector;
            existing.CurrentPrice = updatedAsset.CurrentPrice;
            existing.LastUpdated = DateTime.UtcNow;

            _repository.Update(existing);
            return (true, "Ativo atualizado com sucesso.");
        }

        public (bool success, string message) UpdatePrice(int id, double newPrice)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                return (false, "Ativo não encontrado.");

            if (newPrice <= 0)
                return (false, "Preço deve ser maior que zero.");

            existing.CurrentPrice = newPrice;
            existing.LastUpdated = DateTime.UtcNow;
            _repository.Update(existing);
            return (true, "Preço atualizado com sucesso.");
        }

        public (bool success, string message) Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                return (false, "Ativo não encontrado.");

            _repository.Delete(existing.Symbol);
            return (true, "Ativo excluído com sucesso.");
        }

        public (bool success, string message) Delete(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
