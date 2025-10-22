using Microsoft.AspNetCore.Mvc;
using Portifolio.Models.Models;
using Portifolio.Services.Interfaces;

namespace Portifolio.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _service;

        public AssetsController(IAssetService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os ativos financeiros disponíveis.
        /// </summary>
        /// <remarks>
        /// Retorna a lista completa de ativos cadastrados no sistema.
        /// Esses dados são carregados a partir do arquivo <c>SeedData.json</c>.
        /// </remarks>
        /// <returns>Lista de objetos <see cref="Asset"/>.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        // GET: api/assets
        [HttpGet]
        public ActionResult<IEnumerable<Asset>> GetAll()
        {
            var assets = _service.GetAll();
            return Ok(assets);
        }

        /// <summary>
        /// Obtém um ativo financeiro pelo seu identificador.
        /// </summary>
        /// <param name="id">ID do ativo a ser consultado.</param>
        /// <returns>Retorna o ativo correspondente ao ID informado.</returns>
        /// <response code="200">Ativo encontrado.</response>
        /// <response code="404">Ativo não encontrado.</response>
        [HttpGet("{id:int}")]
        public ActionResult<Asset> GetById(int id)
        {
            var asset = _service.GetById(id);
            return asset == null ? NotFound($"Ativo com ID {id} não encontrado.") : Ok(asset);
        }

        /// <summary>
        /// Busca um ativo pelo seu símbolo (ex: PETR4).
        /// </summary>
        /// <param name="symbol">Símbolo do ativo a ser buscado.</param>
        /// <returns>Retorna o ativo correspondente ao símbolo informado.</returns>
        /// <response code="200">Ativo encontrado.</response>
        /// <response code="404">Ativo não encontrado.</response>
        [HttpGet("search")]
        public ActionResult<Asset> Search([FromQuery] string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Símbolo não pode estar vazio.");

            var asset = _service.GetBySymbol(symbol);
            return asset == null ? NotFound($"Ativo '{symbol}' não encontrado.") : Ok(asset);
        }

        /// <summary>
        /// Cria um novo ativo financeiro.
        /// </summary>
        /// <param name="asset">Objeto contendo as informações do ativo a ser criado.</param>
        /// <returns>Ativo criado com sucesso.</returns>
        /// <response code="201">Ativo criado com sucesso.</response>
        /// <response code="400">Erro de validação nos dados do ativo.</response>
        [HttpPost]
        public IActionResult Create([FromBody] Asset asset)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Create(asset);
            if (!result.success)
                return Conflict(result.message);

            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        /// <summary>
        /// Atualiza o preço atual de um ativo.
        /// </summary>
        /// <param name="id">ID do ativo a ser atualizado.</param>
        /// <param name="newPrice">Novo preço a ser atribuído.</param>
        /// <returns>Retorna 204 se a atualização foi bem-sucedida.</returns>
        /// <response code="204">Preço atualizado com sucesso.</response>
        /// <response code="404">Ativo não encontrado.</response>
        [HttpPut("{id:int}/price")]
        public IActionResult UpdatePrice(int id, [FromBody] double newPrice)
        {
            var result = _service.UpdatePrice(id, newPrice);
            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.message);
        }

    }
}
