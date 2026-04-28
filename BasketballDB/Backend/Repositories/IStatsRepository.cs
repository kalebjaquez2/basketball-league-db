using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Backend.Models;

namespace Backend.Repositories
{
    public interface IStatsRepository
    {
        /// <summary>
        /// Returns players ranked by total games played and
        /// average points per game for a given season.
        /// </summary>
        IReadOnlyList<MostActivePlayer> RetrieveMostActivePlayers(int seasonID);

        /// <summary>
        /// Returns each player's total points, average points per game,
        /// and rank within their team for a given season.
        /// </summary>
        IReadOnlyList<TopScorer> RetrieveTopScorersByTeam(int seasonID);

        /// <summary>
        /// Returns each team's total wins, losses, and average
        /// score per game for a given season.
        /// </summary>
        IReadOnlyList<TeamPerformance> RetrieveTeamPerformance(int seasonID);

        /// <summary>
        /// Returns average player performance stats per game
        /// for a given date range.
        /// </summary>
        IReadOnlyList<GameStatsSummary> RetrieveGameStatsSummary(
            DateOnly startDate, DateOnly endDate);
    }
}