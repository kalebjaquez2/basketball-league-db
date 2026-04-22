using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Backend.Models;

namespace Backend.Repositories
{
    public interface IGameRepository
    {
        /// <summary>
        /// Creates a new game in the database.
        /// </summary>
        Game CreateGame(int homeTeamID, int awayTeamID, int homeTeamScore,
            int awayTeamScore, int courtNumber, int overtimeCount, DateOnly date);

        /// <summary>
        /// Fetches a game by ID. Throws RecordNotFoundException if not found.
        /// </summary>
        Game FetchGame(int gameID);

        /// <summary>
        /// Retrieves all games for a given team.
        /// </summary>
        IReadOnlyList<Game> RetrieveGamesByTeam(int teamID);

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        Game UpdateGame(int gameID, int homeTeamScore,
            int awayTeamScore, int overtimeCount);
    }
}