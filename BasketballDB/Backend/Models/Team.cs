using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Team
    {
        public int TeamID { get; }
        public int SeasonID { get; }
        public string TeamName { get; }

        public Team(int teamID, int seasonID, string teamName)
        {
            TeamID = teamID;
            SeasonID = seasonID;
            TeamName = teamName;
        }
    }
}
