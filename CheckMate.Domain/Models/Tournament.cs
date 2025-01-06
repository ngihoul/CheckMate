namespace CheckMate.Domain.Models
{
    public class Tournament
    {
        public const int STATUS_WAITING_PLAYERS = 1;
        public const int STATUS_IN_PROGRESS = 2;
        public const int STATUS_TERMINATED = 3;

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Place { get; set; }
        public int? NbPlayers { get; set; } = 0;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int? MinElo { get; set; }
        public int? MaxElo { get; set; }
        public IEnumerable<TournamentCategory>? Categories { get; set; }
        public int? Status { get; set; } = 1; // 1 = En attente de joueurs, 2 = En cours, 3 = Terminé
        public int CurrentRound { get; set; } = 0;
        public bool WomenOnly { get; set; } = false;
        public DateTime EndRegistration { get; set; } // >= created_at + min_players
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Cancelled { get; set; } = false;
        public DateTime? CancelledAt { get; set; }
        public IEnumerable<User>? Players { get; set; }
        public IEnumerable<Game>? Games { get; set; }
    }

    public class TournamentPlayerStatus
    {
        public int TounamentId { get; set; }
        public int PlayerId { get; set; }

        public int NbPlayers { get; set; } = 0;
        public bool CanRegister { get; set; } = false;
        public bool IsRegistered { get; set; } = false;
    }

    public class TournamentFilters
    {
        public string? Name { get; set; }
        public string? Place { get; set; }
        public IEnumerable<int>? CategoriesIds { get; set; }
        public int? Status { get; set; }
        public bool? WomenOnly { get; set; }
        public int? Limit { get; set; }
        public int? Page { get; set; }
    }

    public class TournamentResult
    {
        public string Username { get; set; }
        public int nbGames { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public decimal Score { get; set; }
    }
}
