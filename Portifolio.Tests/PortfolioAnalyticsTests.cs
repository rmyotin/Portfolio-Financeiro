using FluentAssertions;
using Moq;
using Portifolio.Models.Models;
using Portifolio.Repositories.Interfaces;
using Portifolio.Services.Interfaces;
using Portifolio.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portifolio.Tests
{
    public class PortfolioAnalyticsTests
    {
        private readonly PortfolioAnalyticsService _service;
        private readonly Mock<IPortfolioService> _portfolioServiceMock = new();
        private readonly Mock<IAssetRepository> _assetRepoMock = new();

        public PortfolioAnalyticsTests()
        {
            _service = new PortfolioAnalyticsService(_portfolioServiceMock.Object, _assetRepoMock.Object);
        }

        // ===============================================================
        // 1️⃣ TESTE DE PERFORMANCE
        // ===============================================================
        [Fact(DisplayName = "Deve calcular corretamente retorno total e anualizado")]
        public void Deve_Calcular_RetornoTotal_E_Anualizado()
        {
            // Arrange
            var data = SeedDataLoader.Load();
            var scenario = data.TestScenarios.First(s => s.Name == "Performance Calculation Test");
            var portfolio = scenario.Portfolio;

            // Simula preços atuais dos ativos
            _assetRepoMock.Setup(x => x.GetBySymbol("WEGE3")).Returns(new Asset { Symbol = "WEGE3", CurrentPrice = 42.85 });
            _assetRepoMock.Setup(x => x.GetBySymbol("TOTS3")).Returns(new Asset { Symbol = "TOTS3", CurrentPrice = 29.40 });

            _portfolioServiceMock.Setup(x => x.CalculateTotalValue(It.IsAny<Portfolio>()))
                .Returns(500 * 42.85 + 300 * 29.40);

            _portfolioServiceMock.Setup(x => x.CalculateReturnPercent(It.IsAny<Portfolio>()))
                .Returns(16.9); // conforme SeedData

            // Act
            var result = _service.GetPerformance(portfolio);

            // Assert
            ((double)result.TotalReturnPercent).Should().BeApproximately(16.9, 0.1);
            ((double)result.AnnualizedReturnPercent).Should().BeInRange(8, 10);
        }

        // ===============================================================
        // 2️⃣ TESTE DE REBALANCEAMENTO
        // ===============================================================
        [Fact(DisplayName = "Deve sugerir BUY e SELL no rebalanceamento")]
        public void Deve_Sugerir_Acoes_De_Rebalanceamento()
        {
            // Arrange
            var data = SeedDataLoader.Load();
            var scenario = data.TestScenarios.First(s => s.Name == "Portfolio Desbalanceado");
            var portfolio = scenario.Portfolio;

            _assetRepoMock.Setup(x => x.GetBySymbol(It.IsAny<string>()))
                .Returns<string>(symbol => data.Assets.First(a => a.Symbol == symbol));

            _portfolioServiceMock.Setup(x => x.CalculateTotalValue(It.IsAny<Portfolio>()))
                .Returns(51050.00);

            // Act
            //var result = _service.GetRebalancing(portfolio) as dynamic;
            var result = _service.GetRebalancing(portfolio);
            var suggestions = result.SuggestedActions.ToList();

            // Assert
            result.IsBalanced.Should().BeFalse();
            suggestions.Should().NotBeEmpty();
            suggestions.Should().Contain(s => s.Action == "SELL");
            suggestions.Should().Contain(s => s.Action == "BUY");
        }

        // ===============================================================
        // 3️⃣ TESTE DE ANÁLISE DE RISCO (Sharpe e Concentração)
        // ===============================================================
        [Fact(DisplayName = "Deve calcular Sharpe Ratio e concentração corretamente")]
        public void Deve_Calcular_Sharpe_E_Concentracao()
        {
            // Arrange
            var data = SeedDataLoader.Load();
            var scenario = data.TestScenarios.First(s => s.Name == "Alto Risco Concentração");
            var portfolio = scenario.Portfolio;

            _assetRepoMock.Setup(x => x.GetBySymbol(It.IsAny<string>()))
                .Returns<string>(symbol => data.Assets.First(a => a.Symbol == symbol));

            _portfolioServiceMock.Setup(x => x.CalculateTotalValue(It.IsAny<Portfolio>()))
                .Returns(80000);

            _portfolioServiceMock.Setup(x => x.CalculateReturnPercent(It.IsAny<Portfolio>()))
                .Returns(8.0);

            // Act
            var result = _service.GetRiskAnalysis(portfolio);
            // Assert
            result.VolatilityPercent.Should().BeGreaterThan(0);
            result.SharpeRatio.Should().BeLessThan(1.0);
            result.LargestAssetConcentrationPercent.Should().BeGreaterThan(70);
        }



    }
}
