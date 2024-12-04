using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Repositories
{
    public class TournamentCategoryRepository : ITournamentCategoryRepository
    {
        private readonly SqlConnection _connection;

        public TournamentCategoryRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<TournamentCategory>> GetAll()
        {
            SqlCommand command = _connection.CreateCommand();
            List<TournamentCategory> categories = new List<TournamentCategory>();

            command.CommandText = "SELECT * FROM [Tournament_category]";

            await _connection.OpenAsync();

            SqlDataReader reader = await command.ExecuteReaderAsync();

            while(reader.Read())
            {
                categories.Add(
                    new TournamentCategory()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Rules = (string)reader["Rules"]
                    }
                ); 
            }

            await _connection.CloseAsync();

            return categories;
        }
    }
}
