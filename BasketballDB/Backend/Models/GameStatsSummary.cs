using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class GameStatsSummary
    {
        public int GameID { get; }
        public DateOnly GameDate { get; }
        public string HomeTeam { get; }
        public string AwayTeam { get; }
        public decimal AveragePoints { get; }
        public decimal AverageRebounds { get; }
        public decimal AverageAssists { get; }
        public decimal AverageTurnovers { get; }

        public GameStatsSummary(int gameID, DateOnly gameDate, string homeTeam,
            string awayTeam, decimal averagePoints, decimal averageRebounds,
            decimal averageAssists, decimal averageTurnovers)
        {
            GameID = gameID;
            GameDate = gameDate;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            AveragePoints = averagePoints;
            AverageRebounds = averageRebounds;
            AverageAssists = averageAssists;
            AverageTurnovers = averageTurnovers;
        }
    }
}