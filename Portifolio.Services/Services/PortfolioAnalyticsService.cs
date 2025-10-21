using Portifolio.Models.Dtos;
using Portifolio.Models.Models;
using Portifolio.Repositories.Interfaces;
using Portifolio.Services.Interfaces;

namespace Portifolio.Services.Services
{
    public class PortfolioAnalyticsService : IPortfolioAnalyticsService
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IAssetRepository _assetRepository;

        private const double SelicRate = 0.12; // 12% a.a.
        private const double TransactionCost = 0.003; // 0.3%

        public PortfolioAnalyticsService(
            IPortfolioService portfolioService,
            IAssetRepository assetRepository)
        {
            _portfolioService = portfolioService;
            _assetRepository = assetRepository;
        }

        public PerformanceResult GetPerformance(Portfolio portfolio)
        {
            double totalValue = _portfolioService.CalculateTotalValue(portfolio);
            double totalReturn = _portfolioService.CalculateReturnPercent(portfolio);

            double days = (DateTime.UtcNow - portfolio.CreatedAt).TotalDays;
            double annualizedReturn = days > 0
                ? Math.Pow(1 + (totalReturn / 100), 365 / days) - 1
                : 0;

            return new PerformanceResult
            (
                portfolio.Name,
            portfolio.TotalInvestment,
            Math.Round(totalValue, 2),
            Math.Round(totalReturn, 2),
            Math.Round(annualizedReturn * 100, 2)
            );
        }

        public RiskAnalysisResult GetRiskAnalysis(Portfolio portfolio)
        {
            // Volatilidade: desvio padrão dos valores das posições
            var values = new List<double>();
            foreach (var pos in portfolio.Positions)
            {
                var asset = _assetRepository.GetBySymbol(pos.AssetSymbol);
                if (asset != null)
                    values.Add(asset.CurrentPrice * pos.Quantity);
            }

            double avg = values.Any() ? values.Average() : 0;
            double variance = values.Any() ? values.Sum(v => Math.Pow(v - avg, 2)) / values.Count : 0;
            double volatility = avg > 0 ? Math.Sqrt(variance) / avg : 0;

            // Sharpe Ratio
            double totalReturn = _portfolioService.CalculateReturnPercent(portfolio) / 100;
            double sharpe = volatility > 0 ? (totalReturn - SelicRate) / volatility : 0;

            // Concentração (maior ativo)
            double totalValue = _portfolioService.CalculateTotalValue(portfolio);
            double largestPositionPercent = 0;

            if (totalValue > 0 && portfolio.Positions.Any())
            {
                var largest = portfolio.Positions
                    .Max(p =>
                    {
                        var asset = _assetRepository.GetBySymbol(p.AssetSymbol);
                        return asset != null ? p.Quantity * asset.CurrentPrice : 0;
                    });

                largestPositionPercent = Math.Round(largest / totalValue * 100, 2);
            }

            return new RiskAnalysisResult(
                portfolio.Name,
                Math.Round(volatility * 100, 2),
                Math.Round(sharpe, 3),
                largestPositionPercent
            );
        }

        public RebalancingResult GetRebalancing(Portfolio portfolio)
        {
            double totalValue = _portfolioService.CalculateTotalValue(portfolio);
            var suggestions = new List<RebalancingAction>();

            foreach (var pos in portfolio.Positions)
            {
                var asset = _assetRepository.GetBySymbol(pos.AssetSymbol);
                if (asset == null) continue;

                double currentValue = pos.Quantity * asset.CurrentPrice;
                double currentWeight = totalValue > 0 ? currentValue / totalValue : 0;
                double difference = currentWeight - pos.TargetAllocation;

                if (Math.Abs(difference) < 0.01) continue; // ignora variações pequenas

                double rebalanceValue = Math.Abs(difference * totalValue);
                double cost = rebalanceValue * 0.003;

                if (rebalanceValue < 100) continue; // ignora transações pequenas

                suggestions.Add(new RebalancingAction(
                    pos.AssetSymbol,
                    difference > 0 ? "SELL" : "BUY",
                    Math.Round(rebalanceValue - cost, 2),
                    Math.Round(cost, 2)
                ));
            }

            bool isBalanced = !suggestions.Any();

            return new RebalancingResult(
                portfolio.Name,
                Math.Round(totalValue, 2),
                isBalanced,
                suggestions
             );
        }

        public object GetDiversification(Portfolio portfolio)
        {
            double totalValue = _portfolioService.CalculateTotalValue(portfolio);

            var diversification = portfolio.Positions
                .GroupBy(p =>
                {
                    var asset = _assetRepository.GetBySymbol(p.AssetSymbol);
                    return asset?.Sector ?? "Unknown";
                })
                .Select(g =>
                {
                    double sectorValue = g.Sum(p =>
                    {
                        var asset = _assetRepository.GetBySymbol(p.AssetSymbol);
                        return asset != null ? p.Quantity * asset.CurrentPrice : 0;
                    });

                    double percent = totalValue > 0 ? (sectorValue / totalValue) * 100 : 0;

                    return new
                    {
                        Sector = g.Key,
                        Value = Math.Round(sectorValue, 2),
                        Percent = Math.Round(percent, 2)
                    };
                })
                .OrderByDescending(s => s.Percent)
                .ToList();

            return new
            {
                Portfolio = portfolio.Name,
                TotalValue = Math.Round(totalValue, 2),
                Sectors = diversification
            };

        }
    }
}
