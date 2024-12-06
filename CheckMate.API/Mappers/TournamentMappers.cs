using CheckMate.API.DTO;
using CheckMate.Domain.Models;

namespace CheckMate.API.Mappers
{
    public static class TournamentMappers
    {
        public static Tournament ToTournament(this TournamentCreateForm tournamentForm)
        {
            return new Tournament
            {
                Name = tournamentForm.Name,
                Place = tournamentForm.Place,
                MinPlayers = tournamentForm.MinPlayers,
                MaxPlayers = tournamentForm.MaxPlayers,
                MinElo = tournamentForm.MinElo,
                MaxElo = tournamentForm.MaxElo,
                WomenOnly   = tournamentForm.WomenOnly,
                EndRegistration = tournamentForm.EndRegistration,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        public static TournamentView ToView(this Tournament tournament, TournamentPlayerStatus? playerStatus = null)
        {
            return new TournamentView
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Place = tournament.Place,
                NbPlayers = playerStatus?.NbPlayers ?? 0,
                MinPlayers = tournament.MinPlayers,
                MaxPlayers = tournament.MaxPlayers,
                MinElo = tournament.MinElo,
                MaxElo = tournament.MaxElo,
                Categories = tournament.Categories,
                Status = tournament.Status,
                CurrentRound = tournament.CurrentRound,
                EndRegistration = tournament.EndRegistration,
                CanRegister = playerStatus?.CanRegister ?? false,
                IsRegistered = playerStatus?.IsRegistered ?? false
            };
        }

        public static TournamentViewList ToViewList(this Tournament tournament, TournamentPlayerStatus? playerStatus = null)
        {
            return new TournamentViewList
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Place = tournament.Place,
                NbPlayers = playerStatus?.NbPlayers ?? 0,
                MinPlayers = tournament.MinPlayers,
                MaxPlayers = tournament.MaxPlayers,
                Categories = tournament.Categories,
                MinElo = tournament.MinElo,
                MaxElo = tournament.MaxElo,
                Status = tournament.Status,
                CurrentRound = tournament.CurrentRound,
                EndRegistration = tournament.EndRegistration,
                CanRegister = playerStatus?.CanRegister ?? false,
                IsRegistered = playerStatus?.IsRegistered ?? false
            };
        }
    }
}
