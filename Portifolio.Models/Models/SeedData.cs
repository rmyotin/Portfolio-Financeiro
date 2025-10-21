namespace Portifolio.Models.Models
{
    public class SeedData
    {
        public List<Asset> Assets { get; set; } = new();
        public List<Portfolio> Portfolios { get; set; } = new();
        public Dictionary<string, List<PriceHistory>> PriceHistory { get; set; } = new();
        public MarketData? MarketData { get; set; }
        public List<TestScenario> TestScenarios { get; set; } = new();
    }

    public class PriceHistory
    {
        public string Date { get; set; } = string.Empty;
        public double Price { get; set; }
    }

    public class MarketData
    {
        public double SelicRate { get; set; }
        public Dictionary<string, IndexPerformance> IndexPerformance { get; set; } = new();
        public List<SectorPerformance> Sectors { get; set; } = new();
    }

    public class IndexPerformance
    {
        public double CurrentValue { get; set; }
        public double DailyChange { get; set; }
        public double MonthlyChange { get; set; }
        public double YearToDate { get; set; }
    }

    public class SectorPerformance
    {
        public string Name { get; set; } = string.Empty;
        public double AverageReturn { get; set; }
        public double Volatility { get; set; }
        public List<string> Assets { get; set; } = new();
    }

    public class TestScenario
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Portfolio Portfolio { get; set; } = new();
        public ExpectedResults? ExpectedResults { get; set; }
    }

    public class ExpectedResults
    {
        public double? TotalValue { get; set; }
        public Dictionary<string, double>? Allocations { get; set; }
        public bool? RebalancingNeeded { get; set; }
        public List<SuggestedAction>? SuggestedActions { get; set; }
        public double? ConcentrationRisk { get; set; }
        public string? RiskLevel { get; set; }
        public List<string>? Alerts { get; set; }
        public double? TotalReturn { get; set; }
        public double? AnnualizedReturn { get; set; }
        public double? SharpeRatio { get; set; }
        public double? Volatility { get; set; }
    }

    public class SuggestedAction
    {
        public string Action { get; set; } = string.Empty;
        public string Asset { get; set; } = string.Empty;
        public double Value { get; set; }
    }
}
