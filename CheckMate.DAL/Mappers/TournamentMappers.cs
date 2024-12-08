using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Mappers
{
    public static class TournamentMappers
    {

        public static Tournament TournamentWithCategories(SqlDataReader reader, IEnumerable<TournamentCategory> categories)
        {
            Tournament tournament = new Tournament
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Place = reader["Place"] is DBNull ? null : (string)reader["Place"],
                MinPlayers = (int)reader["Min_players"],
                MaxPlayers = (int)reader["Max_players"],
                MinElo = (int)reader["Min_elo"],
                MaxElo = (int)reader["Max_elo"],
                Categories = categories,
                Status = Convert.ToInt32(reader["Status"]),
                CurrentRound = (int)reader["Current_round"],
                WomenOnly = (bool)reader["Women_only"],
                EndRegistration = (DateTime)reader["End_registration"],
                CreatedAt = (DateTime)reader["Created_at"],
                UpdatedAt = (DateTime)reader["Updated_at"],
                Cancelled = Convert.ToBoolean(reader["Cancelled"]),
                CancelledAt = reader["Cancelled_at"] is DBNull ? null : (DateTime?)reader["Cancelled_at"]
            };

            return tournament;
        }
    }
}
