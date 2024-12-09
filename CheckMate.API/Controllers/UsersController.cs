﻿using CheckMate.BLL.Interfaces;
using CheckMate.API.DTO;
using CheckMate.API.Mappers;
using CheckMate.BLL.Services;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthService _authService;

        public UsersController(IUserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("api/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserView>> Create([FromBody] UserSelfRegistrationForm userForm)
        {
            if (userForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides");
            }

            User? userToAdd = await _userService.Create(userForm.ToUser());

            return Ok(userToAdd!.ToView());
        }

        [HttpPost("api/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginForm userForm)
        {
            if (userForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides");
            }

            User? user = await _userService.Login(userForm.UsernameOrEmail, userForm.Password);

            if (user is null)
            {
                throw new ArgumentNullException("Données invalides");
            }

            string token = _authService.GenerateToken(user);

            return Ok(token);
        }

        [HttpPatch("api/username/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserView>> ChooseUsername([FromRoute] int id, [FromBody] UserChooseUsernameForm userForm)
        {
            if (userForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides");
            }

            User? userToPatch = await _userService.ChooseUsername(id, userForm.ToUser());

            if (userToPatch is null)
            {
                throw new ArgumentException("Utilisateur non trouvé");
            }

            return Ok(userToPatch.ToView());
        }
    }
}
