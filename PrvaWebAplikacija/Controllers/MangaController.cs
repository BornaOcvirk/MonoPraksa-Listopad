using Microsoft.AspNetCore.Mvc;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Service.Common;
using Microsoft.AspNetCore.Authorization;
using PrvaWebAplikacija.Common;

namespace PrvaWebAplikacija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class MangaController : ControllerBase
    {
        private readonly IMangaService _service;

        public MangaController(IMangaService service)
        {
            _service = service;
        }

        // GET api/Manga
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Manga>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/Manga/{guid}
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Manga>> Get(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        // GET api/Manga/author/Isayama
        [HttpGet("author/{author}")]
        [AllowAnonymous]
        public async Task<IEnumerable<Manga>> GetByAuthor(string author)
        {
            return await _service.GetByAuthorAsync(author);
        }

        // GET api/Manga/studio/{studioGuid}
        [HttpGet("studio/{studioId:guid}")]
        [AllowAnonymous]
        public async Task<IEnumerable<Manga>> GetByStudio(Guid studioId)
        {
            return await _service.GetByStudioAsync(studioId);
        }

        // POST api/Manga (admin only, from the class-level Authorize)
        [HttpPost]
        public async Task<ActionResult<Manga>> Post([FromBody] CreateMangaDto dto)
        {
            return Ok(await _service.AddAsync(dto));
        }
    }
}
