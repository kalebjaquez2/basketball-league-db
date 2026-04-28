using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class TopScorer
    {
        public int TeamID { get; }
        public int PlayerID { get; }
        public string PlayerName { get; }
        public int GamesPlayed { get; }
        public int TotalPoints { get; }
        public decimal AveragePointsPerGame { get; }
        public int TeamRank { get; }

        public TopScorer(int teamID, int playerID, string playerName,
            int gamesPlayed, int totalPoints, decimal averagePointsPerGame, int teamRank)
        {
            TeamID = teamID;
            PlayerID = playerID;
            PlayerName = playerName;
            GamesPlayed = gamesPlayed;
            TotalPoints = totalPoints;
            AveragePointsPerGame = averagePointsPerGame;
            TeamRank = teamRank;
        }
    }
}