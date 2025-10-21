using Microsoft.AspNetCore.Mvc;
using Portifolio.Models.Models;
using Portifolio.Services.Services;

namespace Portifolio.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly PortfolioService _service;

        public PortfoliosController(PortfolioService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Portfolio>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id:int}")]
        public ActionResult<Portfolio> GetById(int id)
        {
            var portfolio = _service.GetById(id);
            if (portfolio == null)
                return NotFound($"Portfólio {id} não encontrado.");

            var total = _service.CalculateTotalValue(portfolio);
            var retorno = _service.CalculateReturnPercent(portfolio);

            return Ok(new
            {
                portfolio.Id,
                portfolio.Name,
                portfolio.UserId,
                portfolio.TotalInvestment,
                ValorAtual = total,
                RetornoPercentual = retorno,
                portfolio.Positions
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] Portfolio portfolio)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Create(portfolio);
            return result.success ? Ok(result.message) : BadRequest(result.message);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Portfolio portfolio)
        {
            var result = _service.Update(id, portfolio);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }

        // ---- POSITIONS ----
        [HttpPost("{id:int}/positions")]
        public IActionResult AddPosition(int id, [FromBody] Position position)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.AddPosition(id, position);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }

        [HttpPut("{portfolioId:int}/positions/{positionId:int}")]
        public IActionResult UpdatePosition(int portfolioId, int positionId, [FromBody] Position position)
        {
            var result = _service.UpdatePosition(positionId, position);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }

        [HttpDelete("{portfolioId:int}/positions/{positionId:int}")]
        public IActionResult DeletePosition(int portfolioId, int positionId)
        {
            var result = _service.RemovePosition(positionId);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }
    }
}
