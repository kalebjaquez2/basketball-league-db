using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Backend.Models;

namespace Backend.Repositories
{
    public interface ITeamRepository
    {
        /// <summary>
        /// Creates a new team in the database.
        /// </summary>
        Team CreateTeam(int seasonID, string teamName);

        /// <summary>
        /// Fetches a team by ID. Throws RecordNotFoundException if not found.
        /// </summary>
        Team FetchTeam(int teamID);

        /// <summary>
        /// Retrieves all teams for a given season.
        /// </summary>
        IReadOnlyList<Team> RetrieveTeamsBySeason(int seasonID);

        /// <summary>
        /// Updates an existing team.
        /// </summary>
        Team UpdateTeam(int teamID, string teamName);

        /// <summary>
        /// Deletes a team from the database.
        /// </summary>
        void DeleteTeam(int teamID);
    }
}