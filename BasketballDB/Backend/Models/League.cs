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
        public string Location { get; } // This stays for the UI to display "City, State"
        public int LocationID { get; }   // Add this so the code knows which ID it belongs to

        public League(int leagueId, string name, string location, int locationId)
        {
            LeagueID = leagueId;
            LeagueName = name;
            Location = location;
            LocationID = locationId;
        }
    }
}
