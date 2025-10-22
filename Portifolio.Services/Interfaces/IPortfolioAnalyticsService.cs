using Portifolio.Models.Dtos;
using Portifolio.Models.Models;

namespace Portifolio.Services.Interfaces
{
    public interface IPortfolioAnalyticsService
    {
        PerformanceResult GetPerformance(Portfolio portfolio);
        RiskAnalysisResult GetRiskAnalysis(Portfolio portfolio);
        RebalancingResult GetRebalancing(Portfolio portfolio);
        object GetDiversification(Portfolio portfolio);
        CorrelationResult GetAssetCorrelations(Portfolio portfolio);
    }
}
