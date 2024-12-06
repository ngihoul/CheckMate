using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.Domain.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int WhiteId { get; set; }
        public int BlackId { get; set; }
        public int Round { get; set; }
        public int Winner { get; set; }
    }
}
