using Portifolio.Models.Models;
using Portifolio.Repositories.Interfaces;
using Portifolio.Services.Interfaces;

namespace Portifolio.Services.Services
{

    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _repository;
        private readonly IAssetRepository _assetRepository;

        public PortfolioService(IPortfolioRepository repository, IAssetRepository assetRepository)
        {
            _repository = repository;
            _assetRepository = assetRepository;
        }

        public IEnumerable<Portfolio> GetAll() => _repository.GetAll();

        public Portfolio? GetById(int id) => _repository.GetById(id);

        public (bool success, string message) Create(Portfolio portfolio)
        {
            _repository.Add(portfolio);
            return (true, "Portfólio criado com sucesso.");
        }

        public (bool success, string message) Update(int id, Portfolio updated)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                return (false, "Portfólio não encontrado.");

            existing.Name = updated.Name;
            existing.TotalInvestment = updated.TotalInvestment;
            _repository.Update(existing);

            return (true, "Portfólio atualizado com sucesso.");
        }

        public (bool success, string message) Delete(int id)
        {
            var existing = _repository.GetById(id);
            if (existing == null)
                return (false, "Portfólio não encontrado.");

            _repository.Delete(existing);
            return (true, "Portfólio excluído com sucesso.");
        }

        public (bool success, string message) AddPosition(int portfolioId, Position position)
        {
            var portfolio = _repository.GetById(portfolioId);
            if (portfolio == null)
                return (false, "Portfólio não encontrado.");

            _repository.AddPosition(portfolioId, position);
            return (true, "Posição adicionada com sucesso.");
        }

        public (bool success, string message) UpdatePosition(int positionId, Position updated)
        {
            var existing = _repository.GetPosition(positionId);
            if (existing == null)
                return (false, "Posição não encontrada.");

            existing.AssetSymbol = updated.AssetSymbol;
            existing.Quantity = updated.Quantity;
            existing.AveragePrice = updated.AveragePrice;
            existing.TargetAllocation = updated.TargetAllocation;
            existing.LastTransaction = DateTime.UtcNow;

            _repository.UpdatePosition(existing);
            return (true, "Posição atualizada com sucesso.");
        }

        public (bool success, string message) RemovePosition(int positionId)
        {
            var existing = _repository.GetPosition(positionId);
            if (existing == null)
                return (false, "Posição não encontrada.");

            _repository.RemovePosition(existing);
            return (true, "Posição removida com sucesso.");
        }

        public double CalculateTotalValue(Portfolio portfolio)
        {
            double total = 0;
            foreach (var pos in portfolio.Positions)
            {
                var asset = _assetRepository.GetBySymbol(pos.AssetSymbol);
                if (asset != null)
                    total += pos.Quantity * asset.CurrentPrice;
            }
            return total;
        }

        public double CalculateReturnPercent(Portfolio portfolio)
        {
            var totalValue = CalculateTotalValue(portfolio);
            if (portfolio.TotalInvestment <= 0)
                return 0;

            return (totalValue - portfolio.TotalInvestment) / portfolio.TotalInvestment * 100;
        }
    }
}
