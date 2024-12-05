using CheckMate.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Mappers
{
    public static class TournamentCategoryMappers
    {
        public static TournamentCategory TournamentCategory(SqlDataReader reader)
        {
            return new TournamentCategory()
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Rules = (string)reader["Rules"]
            };
        }
    }
}
