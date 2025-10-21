using Microsoft.AspNetCore.Mvc;
using Portifolio.Models.Models;
using Portifolio.Services.Interfaces;
using Portifolio.Services.Services;

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

        // GET: api/assets
        [HttpGet]
        public ActionResult<IEnumerable<Asset>> GetAll()
        {
            var assets = _service.GetAll();
            return Ok(assets);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Asset> GetById(int id)
        {
            var asset = _service.GetById(id);
            return asset == null ? NotFound($"Ativo com ID {id} não encontrado.") : Ok(asset);
        }
        /*
        [HttpGet("{symbol}")]
        public ActionResult<Asset> GetBySymbol(string symbol)
        {
            var asset = _service.GetBySymbol(symbol);
            if (asset == null)
                return NotFound($"Ativo '{symbol}' não encontrado.");

            return Ok(asset);
        }*/

        [HttpGet("search")]
        public ActionResult<Asset> Search([FromQuery] string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Símbolo não pode estar vazio.");

            var asset = _service.GetBySymbol(symbol);
            return asset == null ? NotFound($"Ativo '{symbol}' não encontrado.") : Ok(asset);
        }

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
        /*
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Asset updatedAsset)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Update(id, updatedAsset);
            if (!result.success)
                return NotFound(result.message);

            return Ok(result.message);
        }
        */
        [HttpPut("{id:int}/price")]
        public IActionResult UpdatePrice(int id, [FromBody] double newPrice)
        {
            var result = _service.UpdatePrice(id, newPrice);
            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.message);
        }

        /*caso precise deletar um ativo
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);
            if (!result.success)
                return NotFound(result.message);

            return Ok(result.message);
        }
        */
    }
}
