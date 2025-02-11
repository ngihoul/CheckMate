﻿using CheckMate.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CheckMate.API.DTO
{
    public class TournamentView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Place { get; set; }
        public int? NbPlayers { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public IEnumerable<TournamentCategory>? Categories { get; set; }
        public int? MinElo { get; set; }
        public int? MaxElo { get; set; }
        public int? Status { get; set; }
        public int? CurrentRound { get; set; }
        public DateTime EndRegistration { get; set; }
        public bool CanRegister { get; set; }
        public bool IsRegistered { get; set; }
        public IEnumerable<User>? Players { get; set; }
        public IEnumerable<Game>? Games { get; set; }
    }

    public class TournamentViewList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Place { get; set; }
        public int NbPlayers { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public IEnumerable<TournamentCategory>? Categories { get; set; }
        public int? MinElo { get; set; }
        public int? MaxElo { get; set; }
        public int? Status { get; set; }
        public int? CurrentRound { get; set; }
        public DateTime EndRegistration { get; set; }
        public bool CanRegister { get; set; }
        public bool IsRegistered { get; set; }
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
        public IEnumerable<int>? CategoriesIds { get; set; }

        [Required]
        public bool WomenOnly { get; set; }

        [Required]
        public DateTime EndRegistration { get; set; }
    }

    public class TournamentFiltersForm
    {
        [DataType(DataType.Text)]
        public string? Name { get; set; }
        [DataType(DataType.Text)]
        public string? Place { get; set; }
        public IEnumerable<int>? CategoriesIds { get; set; }
        [Range(1,3)]
        public int? Status { get; set; }
        public bool? WomenOnly { get; set; }
        [MaxLength(25)]
        public int? Limit { get; set; }
        public int? Page { get; set; }
    }
}
