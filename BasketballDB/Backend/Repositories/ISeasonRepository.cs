using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Backend.Models;

namespace Backend.Repositories
{
    public interface ISeasonRepository
    {
        /// <summary>
        /// Creates a new season in the database.
        /// </summary>
        Season CreateSeason(int leagueID, DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Fetches a season by ID. Throws RecordNotFoundException if not found.
        /// </summary>
        Season FetchSeason(int seasonID);

        /// <summary>
        /// Retrieves all seasons for a given league.
        /// </summary>
        IReadOnlyList<Season> RetrieveSeasonsByLeague(int leagueID);

        /// <summary>
        /// Updates an existing season.
        /// </summary>
        Season UpdateSeason(int seasonID, DateOnly startDate, DateOnly endDate);
    }
}