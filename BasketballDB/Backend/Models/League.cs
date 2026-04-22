using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class League
    {
        public int LeagueID { get; }
        public string LeagueName { get; }
        public string Location { get; }

        public League(int leagueID, string leagueName, string location)
        {
            LeagueID = leagueID;
            LeagueName = leagueName;
            Location = location;
        }
    }
}
