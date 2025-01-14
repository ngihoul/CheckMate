﻿using CheckMate.DAL.Interfaces;
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
            using SqlCommand command = _connection.CreateCommand();

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

        public async Task<Game?> GetById(int id)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [Game] WHERE [Id] = @id;";

            command.Parameters.AddWithValue("id", id);

            await _connection.OpenAsync();

            Game? game = null;

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                game = new Game()
                {
                    Id = (int)reader["Id"],
                    TournamentId = (int)reader["TournamentId"],
                    WhiteId = (int)reader["WhiteId"],
                    BlackId = (int)reader["BlackId"],
                    Round = Convert.ToInt32(reader["Round"]),
                    Winner = Convert.ToInt32(reader["Winner"])
                };
            }

            await _connection.CloseAsync();

            return game;
        }

        public async Task<List<Game>> GetByTournament(int tournamentId)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [Game] WHERE [TournamentId] = @tournamentId;";

            command.Parameters.AddWithValue("tournamentId", tournamentId);

            await _connection.OpenAsync();

            List<Game> games = new List<Game>();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                games.Add(
                    new Game()
                    {
                        Id = (int)reader["Id"],
                        TournamentId = (int)reader["TournamentId"],
                        WhiteId = (int)reader["WhiteId"],
                        BlackId = (int)reader["BlackId"],
                        Round = Convert.ToInt32(reader["Round"]),
                        Winner = Convert.ToInt32(reader["Winner"])
                    }
                );
            }

            await _connection.CloseAsync();

            return games;
        }

        public async Task<IEnumerable<Game>> GetByRound(int rournamentId, int roundId)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [Game] WHERE [TournamentId] = @tournamentId AND [Round] = @round;";

            command.Parameters.AddWithValue("tournamentId", rournamentId);
            command.Parameters.AddWithValue("round", roundId);

            await _connection.OpenAsync();

            IEnumerable<Game> games = new List<Game>();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read()) {
                games.Append(
                    new Game()
                    {
                        Id = (int)reader["Id"],
                        TournamentId = (int)reader["TournamentId"],
                        WhiteId = (int)reader["WhiteId"],
                        BlackId = (int)reader["BlackId"],
                        Round = Convert.ToInt32(reader["Round"]),
                        Winner = Convert.ToInt32(reader["Winner"])
                    }
                );
            }

            await _connection.CloseAsync();

            return games;
        }

        public async Task<bool> PatchScore(Game game)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "UPDATE [Game] SET [Winner] = @winner WHERE [Id] = @id;";

            command.Parameters.AddWithValue("id", game.Id);
            command.Parameters.AddWithValue("winner", game.Winner);

            await _connection.OpenAsync();

            int rowsAffected = command.ExecuteNonQuery();

            await _connection.CloseAsync();

            return rowsAffected == 1;
        }
    }
}
