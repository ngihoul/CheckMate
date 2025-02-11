﻿using CheckMate.DAL.Interfaces;
using CheckMate.DAL.Mappers;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly SqlConnection _connection;
        private readonly ITournamentCategoryRepository _repositoryCategory;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;

        public TournamentRepository(SqlConnection connection, ITournamentCategoryRepository repositoryCategory, IUserRepository userRepository, IGameRepository gameRepository)
        {
            _connection = connection;
            _repositoryCategory = repositoryCategory;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
        }

        public async Task<Tournament>? Create(Tournament tournament)
        {
            await _connection.OpenAsync();

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    using SqlCommand command = _connection.CreateCommand();
                    command.Transaction = transaction;

                    command.CommandText = "INSERT INTO [Tournament] ([Name], [Place], [Min_players], [Max_players], [Min_elo], [Max_elo], [Status], [Current_round], [Women_only], [End_registration], [Created_at], [Updated_at], [Cancelled], [Cancelled_at]) " +
                                          "OUTPUT Inserted.Id " +
                                          "VALUES (@Name, @Place, @Min_players, @Max_players, @Min_elo, @Max_elo, @Status, @Current_round, @Women_only, @End_registration, @Created_at, @Updated_at,  @Cancelled, @Cancelled_at);";

                    command.Parameters.AddWithValue("Name", tournament.Name);
                    command.Parameters.AddWithValue("Place", tournament.Place is null ? DBNull.Value : tournament.Place);
                    command.Parameters.AddWithValue("Min_players", tournament.MinPlayers);
                    command.Parameters.AddWithValue("Max_players", tournament.MaxPlayers);
                    command.Parameters.AddWithValue("Min_elo", tournament.MinElo);
                    command.Parameters.AddWithValue("Max_elo", tournament.MaxElo);
                    command.Parameters.AddWithValue("Status", tournament.Status);
                    command.Parameters.AddWithValue("Current_round", tournament.CurrentRound);
                    command.Parameters.AddWithValue("Women_only", tournament.WomenOnly);
                    command.Parameters.AddWithValue("End_registration", tournament.EndRegistration);
                    command.Parameters.AddWithValue("Created_at", tournament.CreatedAt);
                    command.Parameters.AddWithValue("Updated_at", tournament.UpdatedAt);
                    command.Parameters.AddWithValue("Cancelled", tournament.Cancelled);
                    command.Parameters.AddWithValue("Cancelled_at", tournament.CancelledAt is null ? DBNull.Value : tournament.CancelledAt);

                    tournament.Id = (int)await command.ExecuteScalarAsync();

                    foreach (TournamentCategory category in tournament.Categories)
                    {
                        command.CommandText = "INSERT INTO [MM_Tournament_Category] ([TournamentId], [CategoryId]) VALUES (@TournamentId, @CategoryId)";
                        command.Parameters.AddWithValue("@TournamentId", tournament.Id);
                        command.Parameters.AddWithValue("@CategoryId", category.Id);

                        await command.ExecuteNonQueryAsync();

                        command.Parameters.Clear();
                    }

                    transaction.Commit();

                    return tournament;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw new Exception(ex.Message, ex);
                }
            }
        }

        public async Task<List<Tournament>> GetLast(TournamentFilters filters)
        {
            using SqlCommand command = _connection.CreateCommand();
            List<Tournament> tournaments = new List<Tournament>();

            // TODO : Add NbPlayers, isRegistered, canRegistered

            command.CommandText = $"SELECT TOP({(filters.Limit is not null ? "@Limit" : "10")}) * FROM [Tournament] " +
                                  $"WHERE [Cancelled] = 0 AND [Status] != 3 " +
                                  $"{(filters.Name is not null ? "AND [Name] LIKE @Name " : "")}" +
                                  $"{(filters.Place is not null ? "AND [Place] LIKE @Place " : "")}" +
                                  $"{(filters.Status is not null ? "AND Status = @Status " : "")}" +
                                  $"{(filters.WomenOnly == true ? "AND WomenOnly = 1 " : "")}" +
                                  $"{(filters.CategoriesIds is not null ? "AND EXISTS (SELECT * FROM [MM_Tournament_Category] WHERE [MM_Tournament_Category].[TournamentId] = [Tournament].[Id] AND [MM_Tournament_Category].[CategoryId] IN (@CategoriesIds)) " : "")}" +
                                  "ORDER BY [Updated_at] DESC";

            if (filters.Limit is not null) command.Parameters.AddWithValue("@Limit", filters.Limit);
            if (filters.Name is not null) command.Parameters.AddWithValue("@Name", $"%{filters.Name}%");
            if (filters.Place is not null) command.Parameters.AddWithValue("@Place", $"%{filters.Place}%");
            if (filters.Status is not null) command.Parameters.AddWithValue("@Status", filters.Status);
            if (filters.CategoriesIds is not null) command.Parameters.AddWithValue("@CategoriesIds", string.Join(",", filters.CategoriesIds));

            await _connection.OpenAsync();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                IEnumerable<TournamentCategory> categories = await _repositoryCategory.GetByTournament((int)reader["Id"]);

                //yield return (
                tournaments.Add(
                    TournamentMappers.TournamentWithCategories(reader, categories)
                );
            }

            await _connection.CloseAsync();

            return tournaments;
        }

        public async Task<Tournament>? GetById(int id)
        {
            // TODO : check if necessary
            if (id <= 0)
            {
                throw new Exception("L'Id n'existe pas");
            }

            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [Tournament] WHERE [Id] = @id";

            command.Parameters.AddWithValue("id", id);

            await _connection.OpenAsync();

            IEnumerable<TournamentCategory> categories = await _repositoryCategory.GetByTournament(id);

            IEnumerable<User> players = await _userRepository.GetByTournament(id);

            IEnumerable<Game> games = await _gameRepository.GetByTournament(id);

            Tournament? tournament = null;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                tournament = TournamentMappers.TournamentWithCategories(reader, categories);
            }

            tournament.Players = players;
            tournament.Games = games;

            await _connection.CloseAsync();

            return tournament;
        }

        public async Task<List<TournamentResult>> GetResult(Tournament tournament, int? round = 0)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [u].[Username], " +
                                    "COUNT(CASE WHEN [g].[Winner] != 1 THEN 1 ELSE NULL END) AS [NbGames], " +
                                    "SUM(CASE WHEN [g].[Winner] = 2 AND [g].[WhiteId] = [u].[Id] THEN 1 WHEN [g].[Winner] = 3 AND [g].[BlackId] = [u].[Id] THEN 1 ELSE 0 END) AS [Wins], " +
                                    "SUM(CASE WHEN [g].[Winner] = 3 AND [g].[WhiteId] = [u].[Id] THEN 1 WHEN [g].[Winner] = 2 AND [g].[BlackId] = [u].[Id] THEN 1 ELSE 0 END) AS [Losses], " +
                                    "SUM(CASE WHEN [g].[Winner] = 4 THEN 1 ELSE 0 END) AS [Draws], " +
                                    "SUM(CASE " +
                                        "WHEN [g].[Winner] = 2 AND [g].[WhiteId] = [u].[Id] THEN 1 " +
                                        "WHEN [g].[Winner] = 3 AND [g].[BlackId] = [u].[Id] THEN 1 " +
                                        "WHEN [g].[Winner] = 4 THEN 0.5 " +
                                        "ELSE 0 " +
                                    "END) AS [Score] " +
                                    "FROM " +
                                        "[User] AS [u] " +
                                    "LEFT JOIN " +
                                        "[Game] AS [g] ON [u].[Id] = [g].[WhiteId] OR [u].[Id] = [g].[BlackId] " +
                                    $"WHERE [TournamentId] = @Id {(round == 0 ? "" : $"AND [Round] = @Round")} " +
                                    "GROUP BY " +
                                        "[u].[Username] " +
                                    "ORDER BY [Score] DESC, [Wins] DESC, [Draws] DESC, [Losses] DESC";

            if (round > 0)
            {
                command.Parameters.AddWithValue("Round", round);
            }

            command.Parameters.AddWithValue("Id", tournament.Id);

            await _connection.OpenAsync();

            List<TournamentResult> results = new List<TournamentResult>();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                results.Add(
                    new TournamentResult()
                    {
                        Username = (string)reader["Username"],
                        nbGames = (int)reader["NbGames"],
                        Wins = (int)reader["Wins"],
                        Losses = (int)reader["Losses"],
                        Draws = (int)reader["Draws"],
                        Score = (decimal)reader["Score"]
                    }
                );
            }

            await _connection.CloseAsync();

            return results;
        }

        public async Task<Tournament?> Update(int id, Tournament tournament)
        {
            using SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE [Tournament] " +
                                  "SET [Current_round] = @Current_round, [Status] = @Status, [Updated_at] = GETDATE() " +
                                  "WHERE [Id] = @Id;";

            command.Parameters.AddWithValue("@Current_round", tournament.CurrentRound);
            command.Parameters.AddWithValue("@Status", tournament.Status);
            command.Parameters.AddWithValue("@Id", id);

            await _connection.OpenAsync();

            int rowsAffected = command.ExecuteNonQuery();

            await _connection.CloseAsync();

            return rowsAffected == 1 ? tournament : null;

        }

        public async Task<bool> Delete(Tournament tournament)
        {

            using SqlCommand command = _connection.CreateCommand();
            // QUESTION : est-ce utile de transmettre tout l'objet Tournament ?
            // TODO : Possibilité d utiliser delete(id) avec delete(tournament) Soit uniquement id par facilite (pas de select)
            command.CommandText = "UPDATE [Tournament] SET [Cancelled] = 1, [Cancelled_at] = GETDATE() WHERE [Id] = @Id";
            command.Parameters.AddWithValue("@Id", tournament.Id);

            _connection.Open();

            int rowsAffected = command.ExecuteNonQuery();

            _connection.Close();

            return rowsAffected == 1;
        }

        public async Task<bool> Register(Tournament tournament, User user)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "INSERT INTO [MM_Tournament_Registration] ([TournamentId], [UserId]) " +
                                  "OUTPUT Inserted.Id " +
                                  "VALUES (@tournamentId, @userId);";

            command.Parameters.AddWithValue("tournamentId", tournament.Id);
            command.Parameters.AddWithValue("UserId", user.Id);

            await _connection.OpenAsync();

            int registrationId = (int)await command.ExecuteScalarAsync();

            await _connection.CloseAsync();

            // QUESTION : que renvoie-t-il si erreur ?
            return registrationId > 0;
        }

        public async Task<bool> Unregister(Tournament tournament, User user)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "DELETE FROM [MM_Tournament_Registration] WHERE [TournamentId] = @tournamentId AND [UserId] = @userId;";

            command.Parameters.AddWithValue("tournamentId", tournament.Id);
            command.Parameters.AddWithValue("userId", user.Id);

            await _connection.OpenAsync();

            int rowsAffected = command.ExecuteNonQuery();

            await _connection.CloseAsync();

            return rowsAffected == 1;
        }

        public async Task<bool> IsRegistered(Tournament tournament, User user)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [MM_Tournament_Registration] WHERE [TournamentId] = @tournamentId AND [UserId] = @userId;";

            command.Parameters.AddWithValue("tournamentId", tournament.Id);
            command.Parameters.AddWithValue("userId", user.Id);

            bool result = false;

            await _connection.OpenAsync();

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                result = true;
            }

            await _connection.CloseAsync();

            return result;
        }

        public async Task<IEnumerable<User>> GetAttendees(Tournament tournament)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Date_of_birth], [U].[Gender], [U].[Elo] FROM [MM_Tournament_Registration] AS TR " +
                                  "INNER JOIN [User] AS U ON TR.[UserId] = U.[Id] " +
                                  "WHERE [TournamentId] = @tournamentId;";

            command.Parameters.AddWithValue("tournamentId", tournament.Id);

            await _connection.OpenAsync();

            IEnumerable<User> users = new List<User>();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                users = users.Append(
                    UserMappers.User(reader)
                );
            }

            await _connection.CloseAsync();

            return users;
        }

        public async Task<int> GetNbAttendees(Tournament tournament)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT COUNT(*) AS [attendes] FROM [MM_Tournament_Registration] WHERE [TournamentId] = @tournamentId;";

            command.Parameters.AddWithValue("tournamentId", tournament.Id);

            await _connection.OpenAsync();

            int nbAttendees = 0;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                nbAttendees = (int)reader["attendes"];
            }

            await _connection.CloseAsync();

            return nbAttendees;
        }

        public async Task<int> GetMaxRound(Tournament tournament)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT MAX([Round]) AS [Max_Round] FROM [Game] WHERE [TournamentId] = @tournamentId;";

            command.Parameters.AddWithValue("tournamentId", tournament.Id);

            await _connection.OpenAsync();

            int maxRound = 0;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                maxRound = Convert.ToInt32(reader["Max_Round"]);
            }

            await _connection.CloseAsync();

            return maxRound;
        }
    }
}
