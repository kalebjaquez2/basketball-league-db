using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Season
    {
        public int SeasonID { get; }
        public int LeagueID { get; }
        public DateOnly StartDate { get; }
        public DateOnly EndDate { get; }

        public Season(int seasonID, int leagueID, DateOnly startDate, DateOnly endDate)
        {
            SeasonID = seasonID;
            LeagueID = leagueID;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
