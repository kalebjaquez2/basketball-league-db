using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Backend.Models;

namespace Backend.Repositories
{
    public interface IPlayerRepository
    {
        /// <summary>
        /// Creates a new player in the database.
        /// </summary>
        Player CreatePlayer(int teamID, int jerseyNumber, string firstName,
            string lastName, int? age, string? height, int? weight);

        /// <summary>
        /// Fetches a player by ID. Throws RecordNotFoundException if not found.
        /// </summary>
        Player FetchPlayer(int playerID);

        /// <summary>
        /// Retrieves all players for a given team.
        /// </summary>
        IReadOnlyList<Player> RetrievePlayersByTeam(int teamID);

        /// <summary>
        /// Updates an existing player.
        /// </summary>
        Player UpdatePlayer(int playerID, int jerseyNumber,
            int? age, string? height, int? weight);

        /// <summary>
        /// Deletes a player from the database.
        /// </summary>
        void DeletePlayer(int playerID);
    }
}