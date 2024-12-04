using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public User? Create(User user)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "INSERT INTO [User] (Username, Email, Password, Salt, Date_of_birth, Gender, Elo, Role) " +
                                  "OUTPUT INSERTED.Id " +      
                                  "VALUES (@Username, @Email, @Password, @Salt, @DateOfBirth, @Gender, @Elo, @RoleId)";

            command.Parameters.AddWithValue("Username", user.Username);
            command.Parameters.AddWithValue("Email", user.Email);
            command.Parameters.AddWithValue("Password", user.Password);
            command.Parameters.AddWithValue("Salt", user.Salt);
            command.Parameters.AddWithValue("DateOfBirth", user.DateOfBirth);
            command.Parameters.AddWithValue("Gender", user.Gender);
            command.Parameters.AddWithValue("Elo", user.Elo);
            command.Parameters.AddWithValue("RoleId", user.Role.Id);

            _connection.Open();

            user.Id = (int)command.ExecuteScalar();

            _connection.Close();

            return user;
        }

        public User? GetByEmail(string email) 
        { 
            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [User] WHERE Email = @Email";
            // JOINTURE AVEC ALIASES

            command.Parameters.AddWithValue("Email", email);

            _connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            User? user = null;

            if(reader.Read())
            {
                user = new User
                {
                    Id = (int)reader["Id"],
                    Username = (string)reader["Username"],
                    Email = (string)reader["Email"],
                    Password = (string)reader["Password"],
                    Salt = (string)reader["Salt"],
                    DateOfBirth = (DateTime)reader["Date_of_birth"],
                    Gender = Convert.ToChar(reader["Gender"]),
                    Elo = (int)reader["Elo"],
                    Role = new Role
                    {
                        Id = (int)reader["RoleId"],
                        Name = (string)reader["RoleName"]
                    }
                };
            }

            _connection.Close();

            return user;
        }

        public User? GetByUsername(string username)
        {
            SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [User] WHERE Username = @Username";

            command.Parameters.AddWithValue("Username", username);

            _connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            User? user = null;

            if(reader.Read()) {
                user = new User
                {
                    Id = (int)reader["Id"],
                    Username = (string)reader["Username"],
                    Email = (string)reader["Email"],
                    Password = (string)reader["Password"],
                    Salt = (string)reader["Salt"],
                    DateOfBirth = (DateTime)reader["Date_of_birth"],
                    Gender = Convert.ToChar(reader["Gender"]),
                    Elo = (int)reader["Elo"],
                    Role = (Role)reader["Role"]
                };
            }

            _connection.Close();

            return user;
        }
    }
}
