using Microsoft.AspNetCore.Mvc;
using Portifolio.Models.Models;
using Portifolio.Services.Interfaces;

namespace Portifolio.Controllers.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão de portfólios de investimento.
    /// </summary>
    /// <remarks>
    /// Permite criar, listar e gerenciar as posições dos portfólios dos usuários.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly IPortfolioService _service;

        public PortfoliosController(IPortfolioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todos os portfólios cadastrados.
        /// </summary>
        /// <remarks>
        /// Utilizado para listar os portfólios existentes no sistema, carregados a partir do SeedData.json.
        /// </remarks>
        /// <returns>Lista de objetos <see cref="Portfolio"/>.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        public ActionResult<IEnumerable<Portfolio>> GetAll()
        {
            return Ok(_service.GetAll());
        }


        /// <summary>
        /// Obtém um portfólio específico pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do portfólio.</param>
        /// <returns>Objeto <see cref="Portfolio"/> correspondente ao ID informado.</returns>
        /// <response code="200">Portfólio encontrado.</response>
        /// <response code="404">Portfólio não encontrado.</response>
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

        /// <summary>
        /// Cria um novo portfólio de investimento.
        /// </summary>
        /// <param name="portfolio">Objeto contendo as informações do novo portfólio.</param>
        /// <returns>Retorna o portfólio criado.</returns>
        /// <response code="201">Portfólio criado com sucesso.</response>
        /// <response code="400">Erro de validação nos dados enviados.</response>
        [HttpPost]
        public IActionResult Create([FromBody] Portfolio portfolio)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Create(portfolio);
            return result.success ? Ok(result.message) : BadRequest(result.message);
        }

        /// <summary>
        /// Adiciona uma nova posição de ativo a um portfólio.
        /// </summary>
        /// <param name="id">Identificador do portfólio.</param>
        /// <param name="position">Objeto contendo os dados da posição a ser adicionada.</param>
        /// <returns>Portfólio atualizado com a nova posição.</returns>
        /// <response code="200">Posição adicionada com sucesso.</response>
        /// <response code="404">Portfólio não encontrado.</response>
        [HttpPost("{id:int}/positions")]
        public IActionResult AddPosition(int id, [FromBody] Position position)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.AddPosition(id, position);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }

        /// <summary>
        /// Atualiza uma posição existente de um portfólio.
        /// </summary>
        /// <param name="id">Identificador do portfólio.</param>
        /// <param name="positionId">Identificador da posição a ser atualizada.</param>
        /// <param name="position">Objeto contendo os novos valores da posição.</param>
        /// <response code="204">Posição atualizada com sucesso.</response>
        /// <response code="404">Portfólio ou posição não encontrada.</response>
        [HttpPut("{portfolioId:int}/positions/{positionId:int}")]
        public IActionResult UpdatePosition(int portfolioId, int positionId, [FromBody] Position position)
        {
            var result = _service.UpdatePosition(positionId, position);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }

        /// <summary>
        /// Remove uma posição de um portfólio.
        /// </summary>
        /// <param name="id">Identificador do portfólio.</param>
        /// <param name="positionId">Identificador da posição a ser removida.</param>
        /// <response code="204">Posição removida com sucesso.</response>
        /// <response code="404">Portfólio ou posição não encontrada.</response>
        [HttpDelete("{portfolioId:int}/positions/{positionId:int}")]
        public IActionResult DeletePosition(int portfolioId, int positionId)
        {
            var result = _service.RemovePosition(positionId);
            return result.success ? Ok(result.message) : NotFound(result.message);
        }
    }
}
