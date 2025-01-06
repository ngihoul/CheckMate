using CheckMate.BLL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CheckMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("{gameId:int:min(1)}/score/{winner:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Game>> setScore(int gameId, int winner)
        {
            Game gameToUpdate = await _gameService.GetById(gameId);

            gameToUpdate.Winner = winner;

            bool isUpdated = await _gameService.PatchScore(gameToUpdate);

            return isUpdated ? Ok(gameToUpdate) : BadRequest("Une erreur est survenue lors de la mise à jour du score");
        }
    }
}
