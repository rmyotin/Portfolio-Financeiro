using Microsoft.AspNetCore.Mvc;
using Portifolio.Repositories.Interfaces;
using Portifolio.Services.Interfaces;

namespace Portifolio.Controllers.Controllers
{
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

        [HttpGet("performance")]
        public IActionResult GetPerformance(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetPerformance(portfolio));
        }

        [HttpGet("risk-analysis")]
        public IActionResult GetRiskAnalysis(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetRiskAnalysis(portfolio));
        }

        [HttpGet("rebalancing")]
        public IActionResult GetRebalancing(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetRebalancing(portfolio));
        }

        [HttpGet("diversification")]
        public IActionResult GetDiversification(int id)
        {
            var portfolio = _portfolioRepository.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            return Ok(_analyticsService.GetDiversification(portfolio));
        }
    }
}
