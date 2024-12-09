using CheckMate.BLL.Interfaces;
using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;

namespace CheckMate.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ITournamentRepository _tournamentRepository;

        public GameService(IGameRepository gameRepository, ITournamentRepository tournamentRepository)
        {
            _gameRepository = gameRepository;
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Game> GetById(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentNullException("L'id du match doit etre supérieur a zéro");
            }

            Game game = await _gameRepository.GetById(id);

            if(game == null)
            {
                throw new ArgumentException("Le match n'existe pas");
            }

            return game;
        }

        public async Task<bool> PatchScore(Game game)
        {
            if(game == null) {
                throw new ArgumentNullException("Le match ne peut pas etre null");
            }

            Tournament? tournament = await _tournamentRepository.GetById(game.TournamentId);

            if(tournament == null)
            {

                throw new ArgumentException("Le tournoi n'existe pas");
            }

            if(tournament.Status != Tournament.STATUS_IN_PROGRESS)
            {
                throw new ArgumentException("Le tournoi n'est pas en cours");
            }

            if(tournament.CurrentRound != game.Round)
            {
                throw new ArgumentException("Le round est incorrect");
            }

            return await _gameRepository.PatchScore(game);
        }
    }
}
