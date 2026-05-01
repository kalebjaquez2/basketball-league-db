using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class PlayerGameStats
    {
        public int PlayerID { get; set; }
        public int GameID { get; set; }
        public int TeamID { get; set; }
        public string? PlayerName { get; set; }
        public int Points => (FieldGoalsMade * 2) + ThreePointersMade;
        public int PlayingTime { get; set; }
        public int Turnovers { get; set; }
        public int Rebounds { get; set; }
        public int Assists { get; set; }
        public int Steals { get; set; }
        public int Blocks { get; set; }
        public int FieldGoalsTaken { get; set; }
        public int FieldGoalsMade { get; set; }
        public int ThreePointersTaken { get; set; }
        public int ThreePointersMade { get; set; }
        public int PersonalFouls { get; set; }

        private string _homeTeamName;
        private string _awayTeamName;

        public string FieldGoalsDisplay => $"{FieldGoalsMade}-{FieldGoalsTaken}";
        public string ThreePointersDisplay => $"{ThreePointersMade}-{ThreePointersTaken}";

        public PlayerGameStats(int playerID, int gameID, int teamID,
            int playingTime, int turnovers, int rebounds, 
            int assists, int steals, int blocks, int fieldGoalsTaken, 
            int fieldGoalsMade, int threePointersTaken, int threePointersMade, 
            int personalFouls)
        {
            PlayerID = playerID;
            GameID = gameID;
            TeamID = teamID;
            PlayingTime = playingTime;
            Turnovers = turnovers;
            Rebounds = rebounds;
            Assists = assists;
            Steals = steals;
            Blocks = blocks;
            FieldGoalsTaken = fieldGoalsTaken;
            FieldGoalsMade = fieldGoalsMade;
            ThreePointersTaken = threePointersTaken;
            ThreePointersMade = threePointersMade;
            PersonalFouls = personalFouls;
        }
    }
}