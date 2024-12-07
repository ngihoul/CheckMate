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

        public async Task<Game> Create(Game game)
        {
            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "INSERT INTO [Game] ([TournamentId], [WhiteId], [BlackId], [Round], [Winner]) " +
                                    "OUTPUT Inserted.Id " +
                                    // TODO : To check if 1 for Winner is ok
                                    "VALUES (@TournamentId, @WhiteId, @BlackId, @Round, 1);";

            command.Parameters.AddWithValue("TournamentId", game.TournamentId);
            command.Parameters.AddWithValue("WhiteId", game.WhiteId);
            command.Parameters.AddWithValue("BlackId", game.BlackId);
            command.Parameters.AddWithValue("Round", game.Round);

            await _connection.OpenAsync();
            game.Id = (int)command.ExecuteScalar();
            await _connection.CloseAsync();

            return game;
        }
    }
}
