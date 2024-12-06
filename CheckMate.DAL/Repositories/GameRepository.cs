using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly SqlConnection _connection;

        public GameRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public Task<Game> Create(Game game)
        {
            SqlCommand sqlCommand = _connection.CreateCommand();

            sqlCommand.CommandText = "INSERT INTO [Game] ([TournamentId], [WhiteId], [BlackId], [Round], [Winner]) " +
                                    "OUTPUT Inserted.Id " +
                                    // To check if 1 for Winner is ok
                                    "VALUES (@TournamentId, @WhiteId, @BlackId, @Round, 1);";
        }
    }
}
