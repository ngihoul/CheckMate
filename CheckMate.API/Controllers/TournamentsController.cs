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
        public async Task<ActionResult<TournamentView>> Get([FromRoute] int id)
        {
            Tournament tournament = await _tournamentService.GetById(id);

            if(tournament is null)
            {
                throw new ArgumentException("Tournoi non trouvé");
            }

            TournamentPlayerStatus? playerStatus = await GetPlayerStatus(tournament);

            return Ok(tournament.ToView(playerStatus));
        }

        [HttpGet]
        [Route("/api/tournaments/last")]
        // TODO : check possible response codes
        // TODO : tout passer en IEnumerable
        public async Task<ActionResult<IEnumerable<TournamentViewList>>> GetLastTournament([FromQuery] TournamentFilters filters)
        {
            List<Tournament> tournaments = await _tournamentService.GetLast(filters);

            if(tournaments.Count == 0)
            {
                throw new Exception("Aucun tournoi trouvé");
            }

            IEnumerable<TournamentViewList> tournamentsView = tournaments.Select( t =>
            {
                TournamentPlayerStatus playerStatus = GetPlayerStatus(t).Result;
                return t.ToViewList(playerStatus);
            });

            return Ok(tournamentsView);
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

        // TODO : check possible response codes 
        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpGet("{tournamentId:int:min(1)}/register/{userId:int:min(1)}")]
        public async Task<bool> Register([FromRoute] int tournamentId, [FromRoute] int userId)
        {
            if(tournamentId <= 0 || userId <= 0)
            {
                throw new ArgumentException("Données invalides");
            }

            return await _tournamentService.Register(tournamentId, userId);
        }

        [HttpGet("{tournamentId:int:min(1)}/unregister/{userId:int:min(1)}")]
        public async Task<bool> Unregister([FromRoute] int tournamentId, [FromRoute] int userId)
        {
            if (tournamentId <= 0 || userId <= 0)
            {
                throw new ArgumentException("Données invalides");
            }

            return await _tournamentService.Unregister(tournamentId, userId);
        }

        [HttpGet("{tournamentId:int:min(1)}/start")]
        public async Task<bool> Start([FromRoute] int tournamentId)
        {
            if(tournamentId <= 0)
            {
                throw new ArgumentException("Données invalides");
            }

            return await _tournamentService.Start(tournamentId);
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
