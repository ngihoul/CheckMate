using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;
using CheckMate.DAL.Mappers;

namespace CheckMate.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<User?> Create(User user)
        {
            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "INSERT INTO [User] (Username, Email, Password, Salt, Date_of_birth, Gender, Elo, RoleId) " +
                                  "OUTPUT INSERTED.Id " +
                                  "VALUES (@Username, @Email, @Password, @Salt, @DateOfBirth, @Gender, @Elo, 1)";

            command.Parameters.AddWithValue("Username", user.Username is null ? DBNull.Value : user.Username);
            command.Parameters.AddWithValue("Email", user.Email);
            command.Parameters.AddWithValue("Password", user.Password);
            command.Parameters.AddWithValue("Salt", user.Salt);
            command.Parameters.AddWithValue("DateOfBirth", user.DateOfBirth);
            command.Parameters.AddWithValue("Gender", user.Gender);
            command.Parameters.AddWithValue("Elo", user.Elo);

            await _connection.OpenAsync();

            user.Id = (int)await command.ExecuteScalarAsync();

            await _connection.CloseAsync();

            return user;
        }

        public async Task<User?> GetByEmail(string email)
        {
            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Date_of_birth], [U].[Gender], [U].[Elo], [U].[RoleId], [R].[Id] AS [RoleId], [R].[Name] AS [RoleName] FROM [User] AS U " +
                                  "JOIN [Role] AS R ON [U].[RoleId] = [R].[Id] " +
                                  "WHERE Email = @Email";

            command.Parameters.AddWithValue("Email", email);

            await _connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            User? user = null;

            if (await reader.ReadAsync())
            {
                user = UserMappers.UserWithRole(reader);
            }

            await _connection.CloseAsync();

            return user;
        }

        public async Task<User?> GetByEmailForLogin(string email)
        {
            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Password], [U].[Salt], [U].[RoleId], [R].[Id] AS [RoleId], [R].[Name] AS [RoleName] FROM [User] AS U " +
                                  "JOIN [Role] AS R ON [U].[RoleId] = [R].[Id] " +
                                  "WHERE Email = @Email";

            command.Parameters.AddWithValue("Email", email);

            await _connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            User? user = null;

            if (await reader.ReadAsync())
            {
                user = UserMappers.UserForLogin(reader);
            }

            await _connection.CloseAsync();

            return user;

        }

        public async Task<User?> GetByUsername(string username)
        {
            // TODO : check if necessary
            if (username is null)
            {
                return null;
            }

            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Date_of_birth], [U].[Gender], [U].[Elo], [R].[Id] AS [RoleId], [R].[Name] AS [RoleName] FROM [User] AS U " +
                                  "JOIN [Role] AS R ON [U].[RoleId] = [R].[Id] " +
                                  "WHERE Username = @Username";

            command.Parameters.AddWithValue("Username", username);

            await _connection.OpenAsync();

            using SqlDataReader reader = command.ExecuteReader();

            User? user = null;

            if (reader.Read())
            {
                user = UserMappers.UserWithRole(reader);
            }

            await _connection.CloseAsync();

            return user;
        }

        public async Task<User?> GetByUsernameForLogin(string username)
        {
            // TODO : check if necessary
            if (username is null)
            {
                return null;
            }

            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Password], [U].[Salt], [U].[RoleId], [R].[Id] AS [RoleId], [R].[Name] AS [RoleName] FROM [User] AS U " +
                                  "JOIN [Role] AS R ON [U].[RoleId] = [R].[Id] " +
                                  "WHERE Username = @Username";

            command.Parameters.AddWithValue("Username", username);

            await _connection.OpenAsync();

            using SqlDataReader reader = command.ExecuteReader();

            User? user = null;

            if (reader.Read())
            {
                user = UserMappers.UserForLogin(reader);
            }

            await _connection.CloseAsync();

            return user;
        }

        public async Task<User?> GetById(int id)
        {
            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Date_of_birth], [U].[Gender], [U].[Elo], [R].[Id] AS [RoleId], [R].[Name] AS [RoleName] FROM [User] AS U " +
                                  "JOIN [Role] AS R ON [U].[RoleId] = [R].[Id] " +
                                  "WHERE [U].[Id] = @Id";

            command.Parameters.AddWithValue("id", id);

            await _connection.OpenAsync();

            using SqlDataReader reader = command.ExecuteReader();

            User? user = null;

            if (reader.Read())
            {
                user = UserMappers.UserWithRole(reader);
            }

            await _connection.CloseAsync();

            return user;
        }

        public async Task<User?> Patch(User user)
        {

            using SqlCommand command = _connection.CreateCommand();
            command.CommandText = @"UPDATE [User] 
                                        SET [Username] = @Username, [Salt] = @Salt, [Password] = @Password
                                        WHERE Id = @Id";

            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Salt", user.Salt);
            command.Parameters.AddWithValue("@Password", user.Password);

            await _connection.OpenAsync();
            int rowsAffected = command.ExecuteNonQuery();
            await _connection.CloseAsync();

            return rowsAffected > 0 ? user : null;
        }

        public async Task<List<User>> GetByCategories(IEnumerable<TournamentCategory> categories)
        {
            SqlCommand command = _connection.CreateCommand();
            List<User> users = new List<User>();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Date_of_birth], [U].[Gender], [U].[Elo], [R].[Id] AS [RoleId], [R].[Name] AS [RoleName] FROM [User] AS U " +
                                  "JOIN [Role] AS R ON [U].[RoleId] = [R].[Id] " +
                                  "WHERE [U].[Id] IS NOT NULL " +
                                  $"AND {(categories.Where(c => c.Name == "Junior").Any() ? "[U].[Date_of_birth] < @minus18yo " : "")} " +
                                  $"AND {(categories.Where(c => c.Name == "Senior").Any() ? "AND [U].[Date_of_birth] BETWEEN @minus18yo AND @minus60yo " : "")} " +
                                  $"AND {(categories.Where(c => c.Name == "Veteran").Any() ? "AND [U].[Date_of_birth] > @minus60yo " : "")}";

            command.Parameters.AddWithValue("@minus18yo", DateTime.Now.AddYears(-18).Date);
            command.Parameters.AddWithValue("@minus60yo", DateTime.Now.AddYears(-60).Date);

            await _connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                users.Add(
                    UserMappers.UserWithRole(reader)
                );
            }

            await _connection.CloseAsync();

            return users;
        }

        public async Task<List<User>> GetByTournament(int tournamentId)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT [U].[Id], [U].[Username], [U].[Email], [U].[Date_of_birth], [U].[Gender], [U].[Elo], [R].[Id] AS [RoleId], [R].[Name] AS [RoleName] FROM [User] AS U " +
                                  "JOIN [Role] AS R ON [U].[RoleId] = [R].[Id] " +
                                  "JOIN [MM_Tournament_Registration] AS TR ON [U].[Id] = [TR].[UserId] " +
                                  "WHERE [TR].[TournamentId] = @TournamentId";

            command.Parameters.AddWithValue("TournamentId", tournamentId);

            await _connection.OpenAsync();

            List<User> users = new List<User>();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(
                    UserMappers.UserWithRole(reader)
                );
            }

            await _connection.CloseAsync();

            return users;
        }
    }
}
