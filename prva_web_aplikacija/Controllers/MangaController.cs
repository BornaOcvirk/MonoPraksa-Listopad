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
        public async Task<IEnumerable<Manga>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/Manga/{guid}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Manga>> Get(Guid id)
        {
            var manga = await _service.GetByIdAsync(id);
            if (manga == null)
                return NotFound();

            return manga;
        }

        // GET api/Manga/author/Isayama
        [HttpGet("author/{author}")]
        public async Task<IEnumerable<Manga>> GetByAuthor(string author)
        {
            return await _service.GetByAuthorAsync(author);
        }

        // GET api/Manga/studio/{studioGuid}
        [HttpGet("studio/{studioId:guid}")]
        public async Task<IEnumerable<Manga>> GetByStudio(Guid studioId)
        {
            return await _service.GetByStudioAsync(studioId);
        }
    }
}
