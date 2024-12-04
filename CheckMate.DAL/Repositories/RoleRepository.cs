using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;

namespace CheckMate.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SqlConnection _connection;

        public RoleRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Role?> GetByName(string name)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [Role] WHERE Name = @Name";

            command.Parameters.AddWithValue("Name", name);

            await _connection.OpenAsync();

            Role? role = null;

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if(reader.Read()) {
                role = new Role
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"]
                };
            }

             await _connection.CloseAsync();

            return role;
        }
    }
}
