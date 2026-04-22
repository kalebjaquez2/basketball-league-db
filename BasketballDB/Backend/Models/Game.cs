using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Game
    {
        public int GameID { get; }
        public int HomeTeamID { get; }
        public int AwayTeamID { get; }
        public int HomeTeamScore { get; }
        public int AwayTeamScore { get; }
        public int CourtNumber { get; }
        public int OvertimeCount { get; }
        public DateOnly Date { get; }

        public Game(int gameID, int homeTeamID, int awayTeamID, int homeTeamScore,
            int awayTeamScore, int courtNumber, int overtimeCount, DateOnly date)
        {
            GameID = gameID;
            HomeTeamID = homeTeamID;
            AwayTeamID = awayTeamID;
            HomeTeamScore = homeTeamScore;
            AwayTeamScore = awayTeamScore;
            CourtNumber = courtNumber;
            OvertimeCount = overtimeCount;
            Date = date;
        }
    }
}
