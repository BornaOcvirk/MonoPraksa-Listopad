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
    public class AnimeWebCatalogueController : ControllerBase
    {
        private readonly IAnimeWebCatalogueService _service;

        public AnimeWebCatalogueController(IAnimeWebCatalogueService service)
        {
            _service = service;
        }

        // GET api/AnimeWebCatalogue
        [HttpGet(Name = "GetAnimes")]
        [AllowAnonymous]
        public async Task<IEnumerable<AnimeWebCatalogue>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/AnimeWebCatalogue/{guid}
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<AnimeWebCatalogue>> Get(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        // GET api/AnimeWebCatalogue/genre/Action?numberOfSeasons=2
        [HttpGet("genre/{genre}")]
        [AllowAnonymous]
        public async Task<IEnumerable<AnimeWebCatalogue>> Get(string genre, int numberOfSeasons)
        {
            return await _service.GetByGenreAsync(genre, numberOfSeasons);
        }

        // GET api/AnimeWebCatalogue/author/Isayama
        [HttpGet("author/{author}")]
        [AllowAnonymous]
        public async Task<IEnumerable<AnimeWebCatalogue>> GetByAuthor(string author)
        {
            return await _service.GetByAuthorAsync(author);
        }

        // GET api/AnimeWebCatalogue/{guid}/price
        [HttpGet("{id:guid}/price")]
        [AllowAnonymous]
        public async Task<ActionResult<decimal>> GetFinalPrice(Guid id)
        {
            return await _service.GetFinalPriceAsync(id);
        }

        // POST api/AnimeWebCatalogue
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimeWebCatalogue new_anime)
        {
            return Ok(await _service.AddAsync(new_anime));
        }

        // PUT api/AnimeWebCatalogue/{guid}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] AnimeWebCatalogue value)
        {
            await _service.UpdateAsync(id, value);
            return Ok();
        }

        // DELETE api/AnimeWebCatalogue/{guid}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}