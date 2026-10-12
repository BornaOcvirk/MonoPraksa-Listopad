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

        public AnimeWebShopController(IAnimeWebShopService service)
        {
            _service = service;
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
            return await _service.GetByIdAsync(id);
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
            return await _service.GetFinalPriceAsync(id);
        }

        // POST api/AnimeWebShop
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimeWebShop new_anime)
        {
            return Ok(await _service.AddAsync(new_anime));
        }

        // PUT api/AnimeWebShop/{guid}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] AnimeWebShop value)
        {
            await _service.UpdateAsync(id, value);
            return Ok();
        }

        // DELETE api/AnimeWebShop/{guid}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}