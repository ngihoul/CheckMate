using CheckMate.BLL.Interfaces;
using CheckMate.BLL.Interfaces;
using CheckMate.DAL.Interfaces;
using CheckMate.DAL.Repositories;
using CheckMate.Domain.Models;

namespace CheckMate.BLL.Services
{
    public class TournamentService : ITournamentService
    {

        private readonly ITournamentCategoryRepository _categoryRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public TournamentService(ITournamentCategoryRepository categoryRepository, ITournamentRepository tournamentRepository, IUserRepository userRepository, IUserService userService)
        {
            _categoryRepository = categoryRepository;
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;
            _userService = userService;
        }

        public async Task<Tournament>? GetById(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("L'id du tournoi doit etre superieur a zéro");
            }

            Tournament? tournament = await _tournamentRepository.GetById(id);

            if(tournament is null || tournament.Cancelled == true)
            {
                throw new ArgumentException("Le tournoi n'existe pas");
            }

            return tournament;
        }

        public async Task<List<Tournament>> GetLast(TournamentFilters filters)
        {
            return await _tournamentRepository.GetLast(filters);
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

                // TODO : A la création d’un tournoi un email est envoyé à tous les joueurs qui respectent les contraintes du tournoi(v.inscriptions) pour les prévenir

                return await _tournamentRepository.Create(tournament);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                Tournament? tournamentToDelete = await _tournamentRepository.GetById(id);

                if (tournamentToDelete is null)
                {
                    throw new Exception("Le tournoi n'existe pas");
                }

                if(tournamentToDelete.EndRegistration < DateTime.Now)
                {
                    throw new Exception("Les inscriptions étant terminées, il n'est pas possible d'annuler le tournoi");
                }
                
                if(tournamentToDelete.Cancelled == true)
                {
                    throw new Exception("Le tournoi a deja ete annulé");
                }

                return await _tournamentRepository.Delete(tournamentToDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TournamentPlayerStatus> GetRegisterInfo(Tournament tournament, int userId)
        {
            User? user = await _userRepository.GetById(userId);

            if (user is null || tournament is null)
            {
                return new TournamentPlayerStatus
                {
                    NbPlayers = 0,
                    TounamentId = tournament.Id,
                    PlayerId = userId,
                    IsRegistered = false,
                    CanRegister = false
                };
            }

            bool isRegistered = await _tournamentRepository.IsRegistered(tournament, user);

            TournamentCategory? userCategory = await _userService.GetUserCategory(user);

            return new TournamentPlayerStatus
            {
                NbPlayers = await _tournamentRepository.GetAttendees(tournament),
                TounamentId = tournament.Id,
                PlayerId = userId,
                IsRegistered = isRegistered,
                CanRegister = tournament.Categories.Any(c => c.Id == userCategory!.Id) && !isRegistered
            };

            // return tournament.Categories.Contains(userCategory);
            //return tournament.Categories.Any(c => c.Id == userCategory.Id);
        }

        public async Task<bool> Register(int tournamentId, int userId)
        {
            Tournament? tournament = await _tournamentRepository.GetById(tournamentId);

            if (tournament == null)
            {
                throw new Exception("Le tournoi n'existe pas");
            }

            User? user = await _userRepository.GetById(userId);

            if (user == null)
            {
                throw new Exception("L'utilisateur n'existe pas");
            }

            if(tournament.EndRegistration < DateTime.Now)
            {
                throw new Exception("Les inscriptions sont terminées");
            }

            int attendees = await _tournamentRepository.GetAttendees(tournament);

            if (attendees >= tournament.MaxPlayers)
            {
                throw new Exception("Le tournoi est complet");
            }

            TournamentCategory userCategory = await _userService.GetUserCategory(user);
            List<TournamentCategory> categories = await _categoryRepository.GetAll();

            if (!categories.Any(c => c.Id == userCategory!.Id))
            {
                throw new Exception("Le tournoi ne correspond pas à votre categorie");
            }

            if (await _tournamentRepository.IsRegistered(tournament, user))
            {
                throw new Exception("L'utilisateur est deja inscrit");
            }

            return await _tournamentRepository.Register(tournament, user);
        }

        private DateTime GetMinimumEndRegistrationDate(Tournament tournament)
        {
            return DateTime.Now.AddDays(tournament.MinPlayers);
        }

        private Task<int> GetAttendees(Tournament tournament) {
            return _tournamentRepository.GetAttendees(tournament);
        }
    }
}
