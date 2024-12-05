using CheckMate.BLL.Interfaces;
using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;

namespace CheckMate.BLL.Services
{
    public class TournamentService : ITournamentService
    {

        private readonly ITournamentCategoryRepository _categoryRepository;
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentCategoryRepository categoryRepository, ITournamentRepository tournamentRepository)
        {
            _categoryRepository = categoryRepository;
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Tournament>? Create(Tournament tournament, List<int> categoriesIds)
        {
            try
            {
                if (tournament.MaxPlayers < tournament.MinPlayers)
                {
                    throw new ArgumentException("Le maximum de joueurs doit etre supérieur ou égal au minimum de joueurs");
                }

                if (tournament.MaxElo < tournament.MinElo)
                {
                    throw new ArgumentException("Le max d'ELO doit etre superieur ou égal au min ELO");
                }

                DateTime minimumEndRegistrationDate = GetMinimumEndRegistrationDate(tournament);

                if (tournament.EndRegistration < minimumEndRegistrationDate)
                {
                    throw new Exception($"La date de fin d'inscription doit etre superieur ou egal au {minimumEndRegistrationDate.ToString("dd/MM/yyyy")} ");
                }

                List<TournamentCategory> categories = await _categoryRepository.GetAll();
                
                tournament.Categories = categories.Where(c => categoriesIds.Contains(c.Id)).ToList();

                return await _tournamentRepository.Create(tournament);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DateTime GetMinimumEndRegistrationDate(Tournament tournament)
        {
            return DateTime.Now.AddDays(tournament.MinPlayers);
        }
    }
}
