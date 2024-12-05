using CheckMate.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CheckMate.API.DTO
{
    public class TournamentView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Place { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int? MinElo { get; set; }
        public int? MaxElo { get; set; }
        public List<TournamentCategory>? Categories { get; set; }
        public int? Status { get; set; }
        public int? CurrentRound { get; set; }
        public bool WomenOnly { get; set; } = false;
        public DateTime EndRegistration { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Cancelled { get; set; }
        public DateTime? CancelledAt { get; set; }
    }

    // TODO : A mettre à jour
    public class TournamentViewList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Place { get; set; }
        public int NbPlayers { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public List<TournamentCategory>? Categories { get; set; }
        public int? MinElo { get; set; }
        public int? MaxElo { get; set; }
        public int? Status { get; set; }
        public int? CurrentRound { get; set; }
        public DateTime EndRegistration { get; set; }
    }

    public class TournamentCreateForm
    {
        [Required]
        public string Name { get; set; }

        public string? Place { get; set; }

        [Required]
        [Range(2, 32)]
        public int MinPlayers { get; set; }

        [Required]
        [Range(2, 32)]
        public int MaxPlayers { get; set; }

        [Range(0, 3000)]
        public int? MinElo { get; set; }

        [Range(0, 3000)]
        public int? MaxElo { get; set; }

        [Required]
        public List<int> CategoriesIds { get; set; }

        [Required]
        public bool WomenOnly { get; set; }

        [Required]
        public DateTime EndRegistration { get; set; }
    }
}
