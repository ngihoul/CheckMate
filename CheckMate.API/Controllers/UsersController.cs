using ChackMate.BLL.Interfaces;
using ChackMate.BLL.Services;
using CheckMate.API.DTO;
using CheckMate.API.Mappers;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("api/register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserView> Create([FromBody] UserSelfRegistrationForm userForm) {
            try
            {
                if (userForm is null || !ModelState.IsValid)
                {
                    return BadRequest(new { message = "Données invalides" });
                }

                User? userToAdd = _userService.Create(userForm.ToUser());

                return Ok(userToAdd.ToView());
            } catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
