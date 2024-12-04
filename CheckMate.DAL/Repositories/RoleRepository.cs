using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SqlConnection _connection;

        public RoleRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public Role? GetByName(string name)
        {
            using SqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM [Role] WHERE Name = @Name";

            command.Parameters.AddWithValue("Name", name);

            _connection.Open();

            Role? role = null;

            using SqlDataReader reader = command.ExecuteReader();

            if(reader.Read()) {
                role = new Role
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"]
                };
            }

             _connection.Close();

            return role;
        }
    }
}
