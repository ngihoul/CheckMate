using CheckMate.DAL.Interfaces;
using CheckMate.DAL.Mappers;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Repositories
{
    public class TournamentCategoryRepository : ITournamentCategoryRepository
    {
        private readonly SqlConnection _connection;

        public TournamentCategoryRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<TournamentCategory>> GetAll()
        {
            using SqlCommand command = _connection.CreateCommand();
            List<TournamentCategory> categories = new List<TournamentCategory>();

            command.CommandText = "SELECT * FROM [Tournament_category]";

            await _connection.OpenAsync();

            SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                categories.Add(
                    new TournamentCategory()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Rules = (string)reader["Rules"]
                    }
                );
            }

            await _connection.CloseAsync();

            return categories;
        }

        public async Task<IEnumerable<TournamentCategory>> GetByTournament(int tournamentId)
        {

            List<TournamentCategory> categories = new List<TournamentCategory>();

            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [MM_Tournament_Category] AS TC " +
                                  "JOIN [Tournament_category] AS C ON TC.CategoryId = C.Id " +
                                  "WHERE TC.TournamentId = @tournamentId";

            command.Parameters.AddWithValue("tournamentId", tournamentId);

            await _connection.OpenAsync();


            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                categories.Add(
                    TournamentCategoryMappers.TournamentCategory(reader)
                );
            }

            await _connection.CloseAsync();

            return categories;
        }
    }
}
