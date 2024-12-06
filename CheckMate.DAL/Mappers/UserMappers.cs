using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Mappers
{
    public static class UserMappers
    {
        public static User UserWithRole(SqlDataReader reader)
        {
            return new User
            {
                Id = (int)reader["Id"],
                Username = reader["Username"] is DBNull ? null : (string)reader["Username"],
                Email = (string)reader["Email"],
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

        public static User UserForLogin(SqlDataReader reader)
        {
            return new User
            {
                Id = (int)reader["Id"],
                Username = reader["Username"] is DBNull ? null : (string)reader["Username"],
                Email = (string)reader["Email"],
                Password = (string)reader["Password"],
                Salt = (string)reader["Salt"],
                Role = new Role
                {
                    Id = (int)reader["RoleId"],
                    Name = (string)reader["RoleName"]
                }
            };
        }
    }
}
