using CheckMate.BLL.Interfaces;
using CheckMate.API.DTO;
using CheckMate.API.Mappers;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CheckMate.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("api/admin/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User?>> CreateByAdmin([FromBody] UserAdminRegistrationForm userForm)
        {
            if (userForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides");
            }

            User? userToAdd = await _userService.CreateByAdmin(userForm.ToUser());

            return Ok(userToAdd);
        }
    }
}
