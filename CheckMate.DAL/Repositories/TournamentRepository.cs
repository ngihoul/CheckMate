using CheckMate.DAL.Interfaces;
using CheckMate.DAL.Mappers;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

// TODO : Try .. catch pas utile dans le repo si pas d'autre opération à part throw

namespace CheckMate.DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly SqlConnection _connection;
        private readonly ITournamentCategoryRepository _repositoryCategory;

        public TournamentRepository(SqlConnection connection, ITournamentCategoryRepository repositoryCategory)
        {
            _connection = connection;
            _repositoryCategory = repositoryCategory;
        }
        public async Task<Tournament>? Create(Tournament tournament)
        {
            await _connection.OpenAsync();

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = _connection.CreateCommand();
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

        public async Task<List<Tournament>> GetLast()
        {
            SqlCommand command = _connection.CreateCommand();
            List<Tournament> tournaments = new List<Tournament>();

            // TODO : Add NbPlayers, isRegistered, canRegistered

            command.CommandText = "SELECT TOP(10) * FROM [Tournament] " +
                                  "WHERE [Cancelled] = 0 AND [Status] != 3 " +
                                  "ORDER BY [Updated_at] DESC";


            await _connection.OpenAsync();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                List<TournamentCategory> categories = await _repositoryCategory.GetByTournament((int)reader["Id"]);

                tournaments.Add(
                    TournamentMappers.TournamentWithCategories(reader, categories)
                );
            }

            await _connection.CloseAsync();

            return tournaments;
        }




        public async Task<Tournament>? GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("L'Id n'existe pas");
                }

                SqlCommand command = _connection.CreateCommand();

                command.CommandText = "SELECT * FROM [Tournament] WHERE [Id] = @id";

                command.Parameters.AddWithValue("id", id);

                await _connection.OpenAsync();

                List<TournamentCategory> categories = await _repositoryCategory.GetByTournament(id);

                Tournament? tournament = null;

                using SqlDataReader reader = command.ExecuteReader();

                // TODO : Utiliser IEnumerable !!!
                if (reader.Read())
                {

                    tournament = TournamentMappers.TournamentWithCategories(reader, categories);
                }

                await _connection.CloseAsync();

                return tournament;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> Delete(Tournament tournament)
        {
            try
            {
                using SqlCommand command = _connection.CreateCommand();
                // QUESTION : est-ce utile de transmettre tout l'objet Tournament ?
                // Possibilité d utiliser delete(id) avec delete(tournament) Soit uniquement id par facilite (pas de select)
                command.CommandText = "UPDATE [Tournament] SET [Cancelled] = 1, [Cancelled_at] = GETDATE() WHERE [Id] = @Id";
                command.Parameters.AddWithValue("@Id", tournament.Id);

                _connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                return rowsAffected == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
