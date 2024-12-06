using CheckMate.BLL.Interfaces;
using CheckMate.API.DTO;
using CheckMate.API.Mappers;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("api/admin/register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User?>> CreateByAdmin([FromBody] UserAdminRegistrationForm userForm)
        {
            try
            {
                if (userForm is null || !ModelState.IsValid)
                {
                    return BadRequest(new { message = "Données invalides" });
                }

                User? userToAdd = await _userService.CreateByAdmin(userForm.ToUser());

                return Ok(userToAdd);
            }
            catch (Exception ex) { 
                return StatusCode(500, new { message = ex.Message });
            
            }
        }
    }
}
