using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    /// <summary>
    /// This helps to show Wins and losses and average points on team cards as well as they're team info
    /// </summary>
    public class TeamWithPerformance
    {
        public int TeamID { get; }
        public int SeasonID { get; }
        public string TeamName { get; }
        public int Wins { get; }
        public int Losses { get; }
        public decimal AverageScorePerGame { get; }

        public TeamWithPerformance(Team team, TeamPerformance? performance)
        {
            TeamID = team.TeamID;
            SeasonID = team.SeasonID;
            TeamName = team.TeamName;
            Wins = performance?.Wins ?? 0;
            Losses = performance?.Losses ?? 0;
            AverageScorePerGame = performance?.AverageScorePerGame ?? 0;
        }
    }
}