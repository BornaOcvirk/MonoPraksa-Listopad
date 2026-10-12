using Microsoft.AspNetCore.Mvc;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Service;
using PrvaWebAplikacija.Service.Common;
using Microsoft.AspNetCore.Authorization;
using PrvaWebAplikacija.Common;

namespace PrvaWebAplikacija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class StudioController : ControllerBase
    {
        private readonly IStudioService _service;

        public StudioController(IStudioService service)
        {
            _service = service;
        }

        // GET api/Studio
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Studio>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/Studio/{guid}
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Studio>> Get(Guid id)
        {
            try
            {
                return await _service.GetStudioByIdAsync(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
