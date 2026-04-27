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
        public int GameID { get; }
        public int TeamID { get; }
        public int Points { get; }
        public int PlayingTime { get; }
        public int Turnovers { get; }
        public int Rebounds { get; }
        public int Assists { get; }

        public PlayerGameStats(int playerID, int gameID, int teamID,
            int points, int playingTime, int turnovers, int rebounds, int assists)
        {
            PlayerID = playerID;
            GameID = gameID;
            TeamID = teamID;
            Points = points;
            PlayingTime = playingTime;
            Turnovers = turnovers;
            Rebounds = rebounds;
            Assists = assists;
        }
    }
}