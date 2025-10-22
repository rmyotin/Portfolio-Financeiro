using Microsoft.AspNetCore.Mvc;
using Portifolio.Repositories.Interfaces;
using Portifolio.Services.Interfaces;

namespace Portifolio.Controllers.Controllers
{
    /// <summary>
    /// Controlador responsável pelos cálculos e análises financeiras de portfólios.
    /// </summary>
    /// <remarks>
    /// Fornece métricas como performance, risco, rebalanceamento e diversificação.
    /// </remarks>
    [ApiController]
    [Route("api/portfolios/{id:int}/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioAnalyticsService _analyticsService;

        public AnalyticsController(IPortfolioRepository portfolioRepository, IPortfolioAnalyticsService analyticsService)
        {
            _portfolioRepository = portfolioRepository;
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Retorna as métricas de performance do portfólio.
        /// </summary>
        /// <param name="id">ID do portfólio.</param>
        /// <returns>
        /// Objeto contendo:
        /// <list type="bullet">
        /// <item><description><b>TotalReturnPercent:</b> Retorno acumulado do portfólio.</description></item>
        /// <item><description><b>AnnualizedReturnPercent:</b> Retorno anualizado com base em 365 dias.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Performance calculada com sucesso.</response>
        /// <response code="404">Portfólio não encontrado.</response>
        [HttpGet("performance")]
        public IActionResult GetPerformance(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetPerformance(portfolio));
        }

        /// <summary>
        /// Retorna a análise de risco do portfólio.
        /// </summary>
        /// <param name="id">ID do portfólio.</param>
        /// <returns>
        /// Objeto contendo:
        /// <list type="bullet">
        /// <item><description><b>VolatilityPercent:</b> Medida de risco (desvio padrão dos retornos).</description></item>
        /// <item><description><b>SharpeRatio:</b> Retorno ajustado ao risco.</description></item>
        /// <item><description><b>LargestAssetConcentrationPercent:</b> Percentual do maior ativo no portfólio.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Análise de risco retornada com sucesso.</response>
        /// <response code="404">Portfólio não encontrado.</response>
        [HttpGet("risk-analysis")]
        public IActionResult GetRiskAnalysis(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetRiskAnalysis(portfolio));
        }

        /// <summary>
        /// Retorna sugestões de rebalanceamento para o portfólio.
        /// </summary>
        /// <param name="id">ID do portfólio.</param>
        /// <returns>
        /// Lista de ações de compra ou venda sugeridas para equilibrar a alocação.
        /// </returns>
        /// <response code="200">Sugestões de rebalanceamento retornadas.</response>
        /// <response code="404">Portfólio não encontrado.</response>
        [HttpGet("rebalancing")]
        public IActionResult GetRebalancing(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetRebalancing(portfolio));
        }

        /// <summary>
        /// Retorna a análise de diversificação do portfólio.
        /// </summary>
        /// <param name="id">ID do portfólio.</param>
        /// <returns>
        /// Percentual de alocação de cada setor econômico dentro do portfólio.
        /// </returns>
        /// <response code="200">Diversificação retornada com sucesso.</response>
        /// <response code="404">Portfólio não encontrado.</response>
        [HttpGet("diversification")]
        public IActionResult GetDiversification(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetDiversification(portfolio));
        }

        /// <summary>
        /// Retorna a matriz de correlação entre os ativos do portfólio.
        /// </summary>
        /// <param name="id">ID do portfólio.</param>
        /// <returns>Coeficientes de correlação de Pearson entre os pares de ativos.</returns>
        /// <response code="200">Matriz de correlação retornada com sucesso.</response>
        /// <response code="404">Portfólio não encontrado.</response>
        [HttpGet("correlations")]
        public IActionResult GetCorrelations(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetAssetCorrelations(portfolio));
        }
    }
}
