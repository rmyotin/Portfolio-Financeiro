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

        [Fact]
        public void Deve_Calcular_RetornoTotal_E_Anualizado_Corretamente()
        {
            // Arrange
            var data = SeedDataLoader.Load();
            var portfolio = data.TestScenarios
                .First(x => x.Name == "Performance Calculation Test")
                .Portfolio;

            // Simula valores atuais via AssetRepository
            _assetRepoMock.Setup(x => x.GetBySymbol("WEGE3")).Returns(new Asset { Symbol = "WEGE3", CurrentPrice = 42.85 });
            _assetRepoMock.Setup(x => x.GetBySymbol("TOTS3")).Returns(new Asset { Symbol = "TOTS3", CurrentPrice = 29.40 });

            _portfolioServiceMock.Setup(x => x.CalculateTotalValue(It.IsAny<Portfolio>()))
                .Returns(500 * 42.85 + 300 * 29.40);

            _portfolioServiceMock.Setup(x => x.CalculateReturnPercent(It.IsAny<Portfolio>()))
                .Returns(16.9); // dado no SeedData.json

            // Act
            var result = _service.GetPerformance(portfolio);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new
            {
                Portfolio = "Teste Performance",
                TotalInvestment = 30000.00,
                CurrentValue = 500 * 42.85 + 300 * 29.40,
                TotalReturnPercent = 16.9,
                AnnualizedReturnPercent = 18.7
            }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void Deve_Sugerir_Buy_Sell_No_Rebalanceamento()
        {
            // Arrange
            var data = SeedDataLoader.Load();
            var portfolio = data.TestScenarios
                .First(x => x.Name == "Portfolio Desbalanceado")
                .Portfolio;

            _assetRepoMock.Setup(x => x.GetBySymbol(It.IsAny<string>()))
                .Returns<string>(s => data.Assets.First(a => a.Symbol == s));

            _portfolioServiceMock.Setup(x => x.CalculateTotalValue(It.IsAny<Portfolio>()))
                .Returns(51050.00);

            // Act
            var result = _service.GetRebalancing(portfolio) as dynamic;

            // Assert
            var actions = ((IEnumerable<object>)result.SuggestedActions).ToList();
            actions.Should().NotBeEmpty();
            actions.Should().Contain(a => a.ToString().Contains("SELL"))
                   .And.Contain(a => a.ToString().Contains("BUY"));
        }

        [Fact]
        public void Deve_Calcular_Sharpe_E_Concentracao_Corretamente()
        {
            // Arrange
            var data = SeedDataLoader.Load();
            var portfolio = data.TestScenarios
                .First(x => x.Name == "Alto Risco Concentração")
                .Portfolio;

            _assetRepoMock.Setup(x => x.GetBySymbol(It.IsAny<string>()))
                .Returns<string>(s => data.Assets.First(a => a.Symbol == s));

            _portfolioServiceMock.Setup(x => x.CalculateTotalValue(It.IsAny<Portfolio>()))
                .Returns(80000);

            _portfolioServiceMock.Setup(x => x.CalculateReturnPercent(It.IsAny<Portfolio>()))
                .Returns(8.0); // retorno de 8%

            // Act
            var result = _service.GetRiskAnalysis(portfolio) as dynamic;

            // Assert
            ((double)result.SharpeRatio).Should().BeLessThan(1.0);
            ((double)result.LargestAssetConcentrationPercent).Should().BeGreaterThan(70);
        }



    }
}
