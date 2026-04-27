using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Player
    {
        public int PlayerID { get; }
        public int TeamID { get; }
        public int JerseyNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int? Age { get; }
        public string? Height { get; }
        public int? Weight { get; }

        public Player(int playerID, int teamID, int jerseyNumber, string firstName,
            string lastName, int? age, string? height, int? weight)
        {
            PlayerID = playerID;
            TeamID = teamID;
            JerseyNumber = jerseyNumber;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Height = height;
            Weight = weight;
        }
    }
}
