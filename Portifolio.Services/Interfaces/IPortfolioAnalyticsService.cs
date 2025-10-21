using Portifolio.Models.Models;

namespace Portifolio.Services.Interfaces
{
    public interface IPortfolioAnalyticsService
    {
        object GetPerformance(Portfolio portfolio);
        object GetRiskAnalysis(Portfolio portfolio);
        object GetRebalancing(Portfolio portfolio);
        object GetDiversification(Portfolio portfolio);
    }
}
