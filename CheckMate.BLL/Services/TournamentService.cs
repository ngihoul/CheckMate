using CheckMate.BLL.Interfaces;
using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;

namespace CheckMate.BLL.Services
{
    public class TournamentService : ITournamentService
    {

        private readonly ITournamentCategoryRepository _categoryRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUserService _userService;
        private readonly MailService _mailService;

        public TournamentService(ITournamentCategoryRepository categoryRepository, ITournamentRepository tournamentRepository, IUserRepository userRepository, IGameRepository gameRepository, IUserService userService, MailService mailService)
        {
            _categoryRepository = categoryRepository;
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _userService = userService;
            _mailService = mailService;
        }

        public async Task<Tournament>? GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException("L'id du tournoi doit etre supérieur a zéro");
            }

            Tournament? tournament = await _tournamentRepository.GetById(id);

            if (tournament is null || tournament.Cancelled == true)
            {
                throw new ArgumentException("Le tournoi n'existe pas");
            }

            return tournament;
        }

        public async Task<IEnumerable<Tournament>> GetLast(TournamentFilters filters)
        {
            return await _tournamentRepository.GetLast(filters);
        }

        public async Task<Tournament>? Create(Tournament tournament, IEnumerable<int> categoriesIds)
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

            IEnumerable<TournamentCategory> categories = await _categoryRepository.GetAll();

            tournament.Categories = categories.Where(c => categoriesIds.Contains(c.Id)).ToList();

            // TODO : Créer une méthode qui check si le joueur est eligible (women, categories & ELO).
            // TODO : refaire envoi de mail aux joueurs eligibles

            /*IEnumerable<User> prospects = _userService.GetByCategories(tournament.Categories);

            foreach (User prospect in prospects)
            {
                _mailService.SendMail(prospect, "Nouveau tournoi pour vous", "Un nouveau tournoi qui pourrait vous intéresser a été ajouté.");
            }*/

            return await _tournamentRepository.Create(tournament);
        }
        
        public async Task<IEnumerable<TournamentResult>> GetResult(int tournamentId, int? round = 0)
        {
            Tournament? tournament = await GetTournament(tournamentId);

            return await _tournamentRepository.GetResult(tournament, round);
        }

        public async Task<bool> Delete(int id)
        {

            Tournament? tournamentToDelete = await GetTournament(id);

            if (tournamentToDelete.EndRegistration < DateTime.Now)
            {
                throw new Exception("Les inscriptions étant terminées, il n'est pas possible d'annuler le tournoi");
            }

            if (tournamentToDelete.Cancelled == true)
            {
                throw new Exception("Le tournoi a deja ete annulé");
            }

            return await _tournamentRepository.Delete(tournamentToDelete);
        }

        public async Task<TournamentPlayerStatus> GetRegisterInfo(Tournament tournament, int userId)
        {
            User? user = await _userRepository.GetById(userId);

            int nbPlayers = await GetNbAttendees(tournament);

            if (user is null || tournament is null)
            {
                return new TournamentPlayerStatus
                {
                    NbPlayers = nbPlayers,
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
                NbPlayers = nbPlayers,
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
            Tournament? tournament = await GetTournament(tournamentId);

            User user = GetUser(userId).Result;

            if (tournament.EndRegistration < DateTime.Now)
            {
                throw new Exception("Les inscriptions sont terminées");
            }

            int attendees = await GetNbAttendees(tournament);

            if (attendees >= tournament.MaxPlayers)
            {
                throw new Exception("Le tournoi est complet");
            }
            // TODO : Son age est calculé par rapport à la date de fin des inscriptions (c-à-d l’âge qu’il aura à la fin des inscriptions)
            TournamentCategory userCategory = await _userService.GetUserCategory(user);
            IEnumerable<TournamentCategory> categories = await _categoryRepository.GetAll();

            if (!categories.Any(c => c.Id == userCategory!.Id))
            {
                throw new Exception("Le tournoi ne correspond pas à votre categorie");
            }

            if (await _tournamentRepository.IsRegistered(tournament, user))
            {
                throw new Exception("L'utilisateur est deja inscrit");
            }

            if (tournament.WomenOnly is true && (user.Gender != 'F' || user.Gender != 'O'))
            {
                throw new Exception("Le tournoi est exclusivement pour les femmes");
            }

            if(tournament.MinElo > user.Elo || tournament.MaxElo < user.Elo)
            {
                throw new Exception("Vous devez avoir un ELO entre " + tournament.MinElo + " et " + tournament.MaxElo);
            }

            return await _tournamentRepository.Register(tournament, user);
        }

        public async Task<bool> Unregister(int tournamentId, int userId)
        {
            Tournament? tournament = await GetTournament(tournamentId);

            User user = GetUser(userId).Result;

            if (!await _tournamentRepository.IsRegistered(tournament, user))
            {
                throw new Exception("L'utilisateur n'est pas inscrit");
            }

            if (tournament.EndRegistration < DateTime.Now)
            {
                throw new Exception("Les inscriptions sont clôturées");
            }

            return await _tournamentRepository.Unregister(tournament, user);
        }

        public async Task<bool> Start(int tournamentId)
        {
            Tournament? tournament = await GetTournament(tournamentId);
            IEnumerable<User> attendees = await _tournamentRepository.GetAttendees(tournament);
            List<User> attendeesList = attendees.ToList(); // Facilite l'accès par index
            int nbAttendees = attendeesList.Count;

            if (tournament.Cancelled == true)
            {
                throw new Exception("Le tournoi a été annulé");
            }

            if(tournament.Status != Tournament.STATUS_WAITING_PLAYERS)
            {
                throw new Exception("Le tournoi est déjà lancé");
            }

            if (tournament.MinPlayers > nbAttendees)
            {
                throw new Exception("Le tournoi n'a pas assez de joueurs");
            }

            /*if (tournament.EndRegistration > DateTime.Now)
            {
                throw new Exception("Les inscriptions ne sont pas clôturées");
            }*/

            if (nbAttendees % 2 != 0)
            {
                attendeesList.Add(new User
                {
                    Id = 0,
                    Username = "BYE"
                });
                nbAttendees++;
            }

            int nbRounds = 2 * (nbAttendees - 1);

            for (int round = 0; round < nbRounds; round++)
            {
                for (int i = 0; i < nbAttendees / 2; i++)
                {
                    User player1 = attendeesList[i];
                    User player2 = attendeesList[nbAttendees - 1 - i];

                    if (player1.Id != 0 && player2.Id != 0)
                    {
                        Game game = new Game
                        {
                            TournamentId = tournamentId,
                            BlackId = round % 2 == 0 ? player1.Id : player2.Id,
                            WhiteId = round % 2 == 0 ? player2.Id : player1.Id,
                            Round = round + 1
                        };

                        await _gameRepository.Create(game);
                    }
                }

                User lastPlayer = attendeesList[nbAttendees - 1];
                attendeesList.RemoveAt(nbAttendees - 1);
                attendeesList.Insert(1, lastPlayer);
            }

            tournament.CurrentRound = 1;
            tournament.Status = Tournament.STATUS_IN_PROGRESS;

            await _tournamentRepository.Update(tournamentId, tournament);

            return true;
        }

        public async Task<Tournament> NextRound(int tournamentId)
        {
            Tournament? tournament = await GetTournament(tournamentId);

            if (tournament.Status == Tournament.STATUS_WAITING_PLAYERS)
            {
                throw new Exception("Le tournoi n'a pas encore commencé");
            }

            if(tournament.Status == Tournament.STATUS_TERMINATED)
            {
                throw new Exception("Le tournoi est déjà clôturé");
            }

            bool allRoundsAreCompleted = await AllRoundsAreCompleted(tournament);

            if (allRoundsAreCompleted is false)
            {
                throw new Exception("La manche n'est pas encore clôturée. Complétez les scores manquants.");
            }

            tournament.CurrentRound++;

            int maxRound = await _tournamentRepository.GetMaxRound(tournament);

            if (maxRound + 1 == tournament.CurrentRound)
            {
                tournament.Status = Tournament.STATUS_TERMINATED;
            }

            Tournament tournamentUpdated = await _tournamentRepository.Update(tournament.Id, tournament);

            return tournamentUpdated;
        }

        private async Task<User> GetUser(int userId)
        {
            User? user = await _userRepository.GetById(userId);

            if (user == null)
            {
                throw new Exception("L'utilisateur n'existe pas");
            }

            return user;
        }

        private async Task<Tournament> GetTournament(int tournamentId)
        {
            Tournament? tournament = await _tournamentRepository.GetById(tournamentId);

            if (tournament == null)
            {
                throw new Exception("Le tournoi n'existe pas");
            }

            return tournament;
        }

        private DateTime GetMinimumEndRegistrationDate(Tournament tournament)
        {
            return DateTime.Now.AddDays(tournament.MinPlayers);
        }

        private Task<int> GetNbAttendees(Tournament tournament)
        {
            return _tournamentRepository.GetNbAttendees(tournament);
        }

        private async Task<bool> AllRoundsAreCompleted(Tournament tournament) {
            IEnumerable<Game> roundGames = await _gameRepository.GetByRound(tournament.Id, tournament.CurrentRound);

            bool completed = true;

            foreach (Game game in roundGames) {
                if(game.Winner == Game.NOT_PLAYED)
                {
                    completed = false;
                }
            }

            return completed;
        }
    }
}
