using Microsoft.AspNetCore.Mvc;
using prva_web_aplikacija.Common;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Service.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/AnimeWebShop
        [HttpGet(Name = "GetAnimes")]
        public IEnumerable<AnimeWebShop> Get()
        {
            return _service.GetAll();
        }

        // GET api/AnimeWebShop/5
        [HttpGet("{id}")]
        public AnimeWebShop Get(int id)
        {
            return _service.GetById(id);
        }

        // GET api/AnimeWebShop/genre/Action?numberOfSeasons=3
        [HttpGet("genre/{genre}")]
        public IEnumerable<AnimeWebShop> Get(string genre, int numberOfSeasons)
        {
            return _service.GetByGenre(genre, numberOfSeasons);
        }

        // GET api/AnimeWebShop/5/price
        [HttpGet("{id}/price")]
        public float? GetFinalPrice(int id)
        {
            return _service.GetFinalPrice(id);
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

        // PUT api/AnimeWebShop/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AnimeWebShop value)
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

        // DELETE api/AnimeWebShop/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _service.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
