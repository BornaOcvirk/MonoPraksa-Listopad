using Microsoft.AspNetCore.Mvc;
using prva_web_aplikacija.Common;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeWebShopController : ControllerBase
    {
        private readonly IAnimeWebShopService _service;

        public AnimeWebShopController(IAnimeWebShopService service)
        {
            _service = service;
        }

        // GET api/AnimeWebShop
        [HttpGet(Name = "GetAnimes")]
        public IEnumerable<AnimeWebShop> Get()
        {
            return _service.GetAll();
        }

        // GET api/AnimeWebShop/{guid}
        [HttpGet("{id:guid}")]
        public ActionResult<AnimeWebShop> Get(Guid id)
        {
            var anime = _service.GetById(id);
            if (anime == null)
                return NotFound();

            return anime;
        }

        // GET api/AnimeWebShop/genre/Action?numberOfSeasons=2
        [HttpGet("genre/{genre}")]
        public IEnumerable<AnimeWebShop> Get(string genre, int numberOfSeasons)
        {
            return _service.GetByGenre(genre, numberOfSeasons);
        }

        // GET api/AnimeWebShop/author/Isayama
        [HttpGet("author/{author}")]
        public IEnumerable<AnimeWebShop> GetByAuthor(string author)
        {
            return _service.GetByAuthor(author);
        }

        // GET api/AnimeWebShop/{guid}/price
        [HttpGet("{id:guid}/price")]
        public ActionResult<decimal> GetFinalPrice(Guid id)
        {
            var price = _service.GetFinalPrice(id);
            if (price == null)
                return NotFound();

            return price.Value;
        }

        // POST api/AnimeWebShop
        [HttpPost]
        public IActionResult Post([FromBody] AnimeWebShop new_anime)
        {
            try
            {
                return Ok(_service.Add(new_anime));
            }
            catch (BuisnessExeptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/AnimeWebShop/{guid}
        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, [FromBody] AnimeWebShop value)
        {
            try
            {
                bool updated = _service.Update(id, value);

                if (!updated)
                    return NotFound();

                return Ok();
            }
            catch (BuisnessExeptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/AnimeWebShop/{guid}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            bool deleted = _service.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}