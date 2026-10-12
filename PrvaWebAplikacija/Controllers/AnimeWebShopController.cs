using Microsoft.AspNetCore.Mvc;
using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Service.Common;
using Microsoft.AspNetCore.Authorization;

namespace PrvaWebAplikacija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class AnimeWebShopController : ControllerBase
    {
        private readonly IAnimeWebShopService _service;
        private readonly ILogger<AnimeWebShopController> _logger;

        public AnimeWebShopController(IAnimeWebShopService service, ILogger<AnimeWebShopController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET api/AnimeWebShop
        [HttpGet(Name = "GetAnimes")]
        [AllowAnonymous]
        public async Task<IEnumerable<AnimeWebShop>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/AnimeWebShop/{guid}
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<AnimeWebShop>> Get(Guid id)
        {
            try
            {
                return await _service.GetByIdAsync(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET api/AnimeWebShop/genre/Action?numberOfSeasons=2
        [HttpGet("genre/{genre}")]
        [AllowAnonymous]
        public async Task<IEnumerable<AnimeWebShop>> Get(string genre, int numberOfSeasons)
        {
            return await _service.GetByGenreAsync(genre, numberOfSeasons);
        }

        // GET api/AnimeWebShop/author/Isayama
        [HttpGet("author/{author}")]
        [AllowAnonymous]
        public async Task<IEnumerable<AnimeWebShop>> GetByAuthor(string author)
        {
            return await _service.GetByAuthorAsync(author);
        }

        // GET api/AnimeWebShop/{guid}/price
        [HttpGet("{id:guid}/price")]
        [AllowAnonymous]
        public async Task<ActionResult<decimal>> GetFinalPrice(Guid id)
        {
            try
            {
                return await _service.GetFinalPriceAsync(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/AnimeWebShop
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimeWebShop new_anime)
        {
            try
            {
                return Ok(await _service.AddAsync(new_anime));
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/AnimeWebShop/{guid}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] AnimeWebShop value)
        {
            try
            {
                await _service.UpdateAsync(id, value);
                return Ok();
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                _logger.LogInformation("PUT anime {Id} finished.", id);
            }
        }

        // DELETE api/AnimeWebShop/{guid}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}