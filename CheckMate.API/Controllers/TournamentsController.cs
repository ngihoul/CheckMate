using CheckMate.API.DTO;
using CheckMate.BLL.Interfaces;
using CheckMate.API.Mappers;
using CheckMate.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Azure.Identity;

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

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TournamentView>> Get([FromRoute] int id)
        {
            Tournament? tournament = await _tournamentService.GetById(id);

            if (tournament is null)
            {
                throw new ArgumentException("Tournoi non trouvé");
            }

            TournamentPlayerStatus? playerStatus = await GetPlayerStatus(tournament);

            return Ok(tournament.ToView(playerStatus));
        }

        [HttpGet]
        [Route("/api/tournaments/last")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TournamentViewList>>> GetLastTournament([FromQuery] TournamentFilters filters)
        {
            IEnumerable<Tournament> tournaments = await _tournamentService.GetLast(filters);

            if (tournaments.Count() == 0)
            {
                throw new ArgumentException("Aucun tournoi trouvé");
            }

            IEnumerable<TournamentViewList> tournamentsView = tournaments.Select(t =>
            {
                TournamentPlayerStatus playerStatus = GetPlayerStatus(t).Result;
                return t.ToViewList(playerStatus);
            });

            return Ok(tournamentsView);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TournamentView>> Create([FromBody] TournamentCreateForm tournamentForm)
        {

            if (tournamentForm is null || !ModelState.IsValid)
            {
                throw new ArgumentNullException("Données invalides");
            }

            Tournament tournamentToAdd = await _tournamentService.Create(tournamentForm.ToTournament(), tournamentForm.CategoriesIds);

            return StatusCode(201, tournamentToAdd.ToView());

        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete([FromRoute] int id)
        {
            bool deleted = await _tournamentService.Delete(id);

            return deleted ? Ok(deleted) : BadRequest(new { message = "Une erreur est survenue lors de la suppression du tournoi" });
        }

        [HttpGet("{tournamentId:int:min(1)}/register/{userId:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Register([FromRoute] int tournamentId, [FromRoute] int userId)
        {
            if (tournamentId <= 0 || userId <= 0)
            {
                throw new ArgumentNullException("Données invalides");
            }

            bool isRegistered = await _tournamentService.Register(tournamentId, userId);

            return isRegistered ? Ok(isRegistered) : BadRequest(new { message = "Une erreur est survenue lors de l'inscription au tournoi" });
        }

        [HttpGet("{tournamentId:int:min(1)}/unregister/{userId:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Unregister([FromRoute] int tournamentId, [FromRoute] int userId)
        {
            if (tournamentId <= 0 || userId <= 0)
            {
                throw new ArgumentNullException("Données invalides");
            }

            bool isUnregistered = await _tournamentService.Unregister(tournamentId, userId);

            return isUnregistered ? Ok(isUnregistered) : BadRequest(new { message = "Une erreur est survenue lors de la désinscription au tournoi" });
        }

        [HttpGet("{tournamentId:int:min(1)}/start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Start([FromRoute] int tournamentId)
        {
            if (tournamentId <= 0)
            {
                throw new ArgumentNullException("Données invalides");
            }

            bool isStarted = await _tournamentService.Start(tournamentId);

            return isStarted ? Ok(isStarted) : BadRequest(new { message = "Une erreur est survenue lors du lancement du tournoi" });
        }

        private async Task<TournamentPlayerStatus> GetPlayerStatus(Tournament tournament)
        {
            int userId = 0;

            if (HttpContext.User.Identity.IsAuthenticated is true)
            {
                userId = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            return await _tournamentService.GetRegisterInfo(tournament, userId);
        }
    }
}
