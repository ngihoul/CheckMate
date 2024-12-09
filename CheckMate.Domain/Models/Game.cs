using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.Domain.Models
{
    public class Game
    {
        public const int NOT_PLAYED = 1;
        public const int WINNER_WHITE = 2;
        public const int WINNER_BLACK = 3;
        public const int DRAW = 4;

        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int WhiteId { get; set; }
        public int BlackId { get; set; }
        public int Round { get; set; }
        public int Winner { get; set; } // 1 = not played, 2 = white, 3 = black, 4 = draw
    }
}
