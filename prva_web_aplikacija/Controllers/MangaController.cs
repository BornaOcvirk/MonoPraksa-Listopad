using Microsoft.AspNetCore.Mvc;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly IMangaService _service;

        public MangaController(IMangaService service)
        {
            _service = service;
        }

        // GET api/Manga
        [HttpGet]
        public IEnumerable<Manga> Get()
        {
            return _service.GetAll();
        }

        // GET api/Manga/{guid}
        [HttpGet("{id:guid}")]
        public ActionResult<Manga> Get(Guid id)
        {
            var manga = _service.GetById(id);
            if (manga == null)
                return NotFound();

            return manga;
        }

        // GET api/Manga/author/Isayama
        [HttpGet("author/{author}")]
        public IEnumerable<Manga> GetByAuthor(string author)
        {
            return _service.GetByAuthor(author);
        }

        // GET api/Manga/studio/{studioGuid}
        [HttpGet("studio/{studioId:guid}")]
        public IEnumerable<Manga> GetByStudio(Guid studioId)
        {
            return _service.GetByStudio(studioId);
        }
    }
}
