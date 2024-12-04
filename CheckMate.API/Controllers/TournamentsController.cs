using CheckMate.API.DTO;
using CheckMate.API.Mappers;
using CheckMate.BLL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TournamentView>> Create([FromBody] TournamentCreateForm tournamentForm)
        {
            try
            {
                if (tournamentForm is null || !ModelState.IsValid)
                {
                    return BadRequest(new { message = "Données invalides" });
                }

                Tournament? tournamentToAdd = await _tournamentService.Create(tournamentForm.ToTournament(), tournamentForm.CategoriesIds);

                return Ok(tournamentToAdd.ToView());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
