using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Service.Common;

namespace PrvaWebAplikacija.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        // GET api/user/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        // DELETE api/user/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // An admin deleting their own account would lock them out
            if (Guid.TryParse(User.FindFirst("sub")?.Value, out var currentUserId) && currentUserId == id)
                return BadRequest("Ne možete obrisati vlastiti račun.");

            var deleted = await _userService.DeleteAsync(id);
            if (deleted ==null) return NotFound();
            return NoContent();
        }
    }
}
