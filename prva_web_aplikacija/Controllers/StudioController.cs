using Microsoft.AspNetCore.Mvc;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Service;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioController : ControllerBase
    {
        private readonly IStudioService _service;

        public StudioController(IStudioService service)
        {
            _service = service;
        }

        // GET api/Studio
        [HttpGet]
        public async Task<IEnumerable<Studio>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/Studio/{guid}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Studio>> Get(Guid id)
        {
            var studio = await _service.GetStudioByIdAsync(id);
            if (studio == null)
                return NotFound();

            return studio;
        }
    }
}
