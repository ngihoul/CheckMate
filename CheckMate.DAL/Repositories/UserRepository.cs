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
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<User?> GetByEmail(string email)
        {
            try
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User?> GetByEmailForLogin(string email)
        {
            try
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User?> GetByUsername(string username)
        {
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<User?> GetByUsernameForLogin(string username)
        {
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<User?> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("L'Id n'existe pas");
                }

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User?> Patch(User user)
        {
            try
            {
                using SqlCommand command = _connection.CreateCommand();
                command.CommandText = @"UPDATE [User] 
                                        SET Username = @Username 
                                        WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Username", user.Username);

                await _connection.OpenAsync();
                int rowsAffected = command.ExecuteNonQuery();
                await _connection.CloseAsync();

                return rowsAffected > 0 ? user : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
