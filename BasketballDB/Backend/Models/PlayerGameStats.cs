using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class PlayerGameStats
    {
        public int PlayerID { get; }
        public int Points { get; }
        public int PlayingTime { get; }
        public int Turnovers { get; }
        public int Rebounds { get; }
        public int Assists { get; }

        publics PlayerGameStats(int playerID, int points, int playingTime,
            int turnovers, int rebounds, int assists)
        {
            PlayerID = playerID;
            Points = points;
            PlayingTime = playingTime;
            Turnovers = turnovers;
            Rebounds = rebounds;
            Assists = assists;
        }
    }
}
