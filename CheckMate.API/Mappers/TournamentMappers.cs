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

        public static TournamentView ToView(this Tournament tournament)
        {
            return new TournamentView
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Place = tournament.Place,
                MinPlayers = tournament.MinPlayers,
                MaxPlayers = tournament.MaxPlayers,
                MinElo = tournament.MinElo,
                MaxElo = tournament.MaxElo,
                Categories = tournament.Categories,
                Status = tournament.Status,
                CurrentRound = tournament.CurrentRound,
                WomenOnly = tournament.WomenOnly,
                EndRegistration = tournament.EndRegistration,
                CreatedAt = tournament.CreatedAt,
                UpdatedAt = tournament.UpdatedAt,
                Cancelled = tournament.Cancelled,
                CancelledAt = tournament.CancelledAt
            };
        }
    }
}
