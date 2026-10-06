using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prva_web_aplikacija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeWebShopController : ControllerBase
    {
        private static List<AnimeWebShop> animelist = new List<AnimeWebShop>
        {
            new AnimeWebShop(1, "Naruto", "Shounen", 2, 19.99f),
            new AnimeWebShop(2, "One Piece", "Adventure", 5, 29.99f),
            new AnimeWebShop(3, "Attack on Titan", "Action", 4, 24.99f),
            new AnimeWebShop(4, "My Hero Academia", "Superhero", 3, 14.99f),
            new AnimeWebShop(5, "Demon Slayer", "Action", 2, 19.99f),
        };

        private static int nextId = animelist.Count + 1;



        // GET: api/<AnimeWebShopController>
        [HttpGet(Name = "GetAnimes")]
        public IEnumerable<AnimeWebShop> Get()
        {
            return animelist;
        }

        // GET api/<AnimeWebShopController>/5
        [HttpGet("{id}")]
        public AnimeWebShop Get(int id)
        {
            for (int i = 0; i < animelist.Count; i++)
            {
                if (animelist[i].Id == id)
                {
                    return animelist[i];
                }
            }
            return null;
        }

        // GET api/<AnimeWebShopController>/genre/NumberOfSeasons
        [HttpGet("genre/{genre}")]
        public IEnumerable<AnimeWebShop> Get(string genre, int numberOfSeasons)
        {
            List<AnimeWebShop> filteredlist = new List<AnimeWebShop>();
            foreach (var anime in animelist)
            {
                if (anime.Genre == genre && anime.NumberOfSeasons >= numberOfSeasons)
                {
                    filteredlist.Add(anime);
                }
            }

            return filteredlist;
        }

    

        // POST api/<AnimeWebShopController>
        [HttpPost]
        public IActionResult Post([FromBody] AnimeWebShop new_anime)
        {
            new_anime.Id = nextId;
            nextId++;
            while(nextId - animelist.Count > 1)
            {
                nextId--;
            };
            animelist.Add(new_anime);
            return Ok(new_anime);
        }

        // PUT api/<AnimeWebShopController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] AnimeWebShop value)
        {
            for (int i = 0; i < animelist.Count; i++)
            {
                if (animelist[i].Id == id)
                {
                    animelist[i].Name = value.Name;
                    animelist[i].Genre = value.Genre;
                    animelist[i].NumberOfSeasons = value.NumberOfSeasons;
                    animelist[i].Price = value.Price;
                    return;
                }
            }
        }

        // DELETE api/<AnimeWebShopController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            for (int i = 0; i < animelist.Count; i++)
            {
                if (animelist[i].Id == id)
                {
                    animelist.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
