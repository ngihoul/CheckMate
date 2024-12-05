using CheckMate.API.DTO;
using CheckMate.API.Mappers;
using CheckMate.BLL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    // Middleware qui gère les erreurs et les différents types d'erreur

    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        [Route("/api/tournaments/last")]
        public async Task<ActionResult<List<TournamentViewList>>> GetLastTournament()
        {
            List<Tournament> tournaments = await _tournamentService.GetLast();

            if(tournaments.Count == 0)
            {
                return NoContent();
            }

            return Ok(tournaments.Select(t => t.ToViewList()).ToList());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TournamentView>> Create([FromBody] TournamentCreateForm tournamentForm)
        {

            if (tournamentForm is null || !ModelState.IsValid)
            {
                throw new ArgumentException("Données invalides");
            }

            Tournament? tournamentToAdd = await _tournamentService.Create(tournamentForm.ToTournament(), tournamentForm.CategoriesIds);

            return Ok(tournamentToAdd.ToView());

        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<bool>> Delete([FromRoute] int id)
        {
            try
            {
                bool deleted = await _tournamentService.Delete(id);

                return deleted ? Ok(deleted) : BadRequest(new { message = "Une erreur est survenue lors de la suppression du tournoi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
