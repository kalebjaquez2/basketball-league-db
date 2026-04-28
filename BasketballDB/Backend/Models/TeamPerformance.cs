using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Models
{
    public class TeamPerformance
    {
        public int TeamID { get; }
        public string TeamName { get; }
        public int Wins { get; }
        public int Losses { get; }
        public decimal AverageScorePerGame { get; }

        public TeamPerformance(int teamID, string teamName,
            int wins, int losses, decimal averageScorePerGame)
        {
            TeamID = teamID;
            TeamName = teamName;
            Wins = wins;
            Losses = losses;
            AverageScorePerGame = averageScorePerGame;
        }
    }
}