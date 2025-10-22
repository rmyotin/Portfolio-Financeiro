using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Models.Dtos
{
    public record PerformanceResult(
        string Portfolio,
        double TotalInvestment,
        double CurrentValue,
        double TotalReturnPercent,
        double AnnualizedReturnPercent);

    public record RebalancingAction(
        string Asset,
        string Action,
        double Value,
        double TransactionCost);

    public record RebalancingResult(
        string Portfolio,
        double TotalValue,
        bool IsBalanced,
        IEnumerable<RebalancingAction> SuggestedActions);

    public record RiskAnalysisResult(
        string Portfolio,
        double VolatilityPercent,
        double SharpeRatio,
        double LargestAssetConcentrationPercent);

    /// <summary>
    /// Representa a correlação estatística entre ativos de um portfólio.
    /// </summary>
    public record CorrelationResult(
        string Portfolio,
        List<AssetCorrelation> Correlations);

    /// <summary>
    /// Correlação de Pearson entre dois ativos.
    /// </summary>
    public record AssetCorrelation(
        string AssetA,
        string AssetB,
        double CorrelationCoefficient);
}
