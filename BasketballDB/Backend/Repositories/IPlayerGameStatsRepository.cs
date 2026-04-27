using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Backend.Models;

namespace Backend.Repositories
{
    public interface IPlayerGameStatsRepository
    {
        /// <summary>
        /// Creates a new player game stats entry in the database.
        /// </summary>
        void CreatePlayerGameStats(int playerID, int gameID, int teamID,
            int points, int playingTime, int turnovers, int rebounds, int assists);

        /// <summary>
        /// Fetches stats for a specific player in a specific game.
        /// Throws RecordNotFoundException if not found.
        /// </summary>
        PlayerGameStats FetchPlayerGameStats(int playerID, int gameID);

        /// <summary>
        /// Retrieves all player stats for a given game.
        /// </summary>
        IReadOnlyList<PlayerGameStats> RetrieveStatsByGame(int gameID);

        /// <summary>
        /// Retrieves all stats for a given player.
        /// </summary>
        IReadOnlyList<PlayerGameStats> RetrieveStatsByPlayer(int playerID);

        /// <summary>
        /// Updates an existing player game stats entry.
        /// </summary>
        PlayerGameStats UpdatePlayerGameStats(int playerID, int gameID,
            int points, int playingTime, int turnovers, int rebounds, int assists);

        /// <summary>
        /// Deletes a player game stats entry.
        /// </summary>
        void DeletePlayerGameStats(int playerID, int gameID);
    }
}