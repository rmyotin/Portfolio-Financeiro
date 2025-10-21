using Microsoft.AspNetCore.Mvc;
using Portifolio.Models.Models;
using Portifolio.Services.Services;

namespace Portifolio.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly AssetService _service;

        public AssetsController(AssetService service)
        {
            _service = service;
        }

        // GET: api/assets
        [HttpGet]
        public ActionResult<IEnumerable<Asset>> GetAll()
        {
            var assets = _service.GetAll();
            return Ok(assets);
        }

        // GET: api/assets/PETR4
        [HttpGet("{symbol}")]
        public ActionResult<Asset> GetBySymbol(string symbol)
        {
            var asset = _service.GetBySymbol(symbol);
            if (asset == null)
                return NotFound($"Ativo '{symbol}' não encontrado.");

            return Ok(asset);
        }

        // POST: api/assets
        [HttpPost]
        public ActionResult<Asset> Create([FromBody] Asset asset)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _service.Create(asset);
            if (!created)
                return Conflict($"O ativo '{asset.Symbol}' já existe.");

            return CreatedAtAction(nameof(GetBySymbol), new { symbol = asset.Symbol }, asset);
        }

        // PUT: api/assets/PETR4
        [HttpPut("{symbol}")]
        public IActionResult Update(string symbol, [FromBody] Asset updatedAsset)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _service.Update(symbol, updatedAsset);
            if (!updated)
                return NotFound($"Ativo '{symbol}' não encontrado.");

            return NoContent();
        }

        // DELETE: api/assets/PETR4
        [HttpDelete("{symbol}")]
        public IActionResult Delete(string symbol)
        {
            var deleted = _service.Delete(symbol);
            if (!deleted)
                return NotFound($"Ativo '{symbol}' não encontrado.");

            return NoContent();
        }
    }
}
