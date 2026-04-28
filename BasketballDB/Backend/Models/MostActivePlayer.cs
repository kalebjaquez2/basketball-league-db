using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class MostActivePlayer
    {
        public int PlayerID { get; }
        public string PlayerName { get; }
        public int TeamID { get; }
        public int GamesPlayed { get; }
        public int TotalPoints { get; }
        public decimal PointsPerGame { get; }
        public int SeasonRank { get; }

        public MostActivePlayer(int playerID, string playerName, int teamID,
            int gamesPlayed, int totalPoints, decimal pointsPerGame, int seasonRank)
        {
            PlayerID = playerID;
            PlayerName = playerName;
            TeamID = teamID;
            GamesPlayed = gamesPlayed;
            TotalPoints = totalPoints;
            PointsPerGame = pointsPerGame;
            SeasonRank = seasonRank;
        }
    }
}