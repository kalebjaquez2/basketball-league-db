using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repositories
{
    public interface ILeagueRepository
    {
        /// <summary>
        /// Creates a new league in the database.
        /// </summary>
        League CreateLeague(string leagueName, int locationID);

        /// <summary>
        /// Fetches a league by ID. Throws RecordNotFoundException if not found.
        /// </summary>
        League FetchLeague(int leagueID);

        /// <summary>
        /// Retrieves all leagues in the database.
        /// </summary>
        IReadOnlyList<League> RetrieveLeagues();

        /// <summary>
        /// Updates an existing league.
        /// </summary>
        League UpdateLeague(int leagueID, string leagueName, int locationId);

        /// <summary>
        /// Soft-deletes a league by ID.
        /// </summary>
        void DeleteLeague(int leagueID);
    }
}
