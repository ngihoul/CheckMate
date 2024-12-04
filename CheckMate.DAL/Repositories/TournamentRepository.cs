using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly SqlConnection _connection;

        public TournamentRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<Tournament>? Create(Tournament tournament)
        {
            try
            {
                await _connection.OpenAsync();

                using (var transaction = _connection.BeginTransaction())
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
                    command.Parameters.AddWithValue("Cancelled", tournament.Cancelled is null ? DBNull.Value : tournament.Cancelled );
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
