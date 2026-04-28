using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Backend.Models;

namespace Backend.Repositories
{
    public class SqlStatsRepository(SqlCommandExecutor executor)
        : IStatsRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public IReadOnlyList<MostActivePlayer> RetrieveMostActivePlayers(int seasonID)
        {
            return executor.ExecuteReader(
                new RetrieveMostActivePlayersDelegate(seasonID));
        }

        public IReadOnlyList<TopScorer> RetrieveTopScorersByTeam(int seasonID)
        {
            return executor.ExecuteReader(
                new RetrieveTopScorersByTeamDelegate(seasonID));
        }

        public IReadOnlyList<TeamPerformance> RetrieveTeamPerformance(int seasonID)
        {
            return executor.ExecuteReader(
                new RetrieveTeamPerformanceDelegate(seasonID));
        }

        public IReadOnlyList<GameStatsSummary> RetrieveGameStatsSummary(
            DateOnly startDate, DateOnly endDate)
        {
            return executor.ExecuteReader(
                new RetrieveGameStatsSummaryDelegate(startDate, endDate));
        }

        // ── Delegates ──────────────────────────────────────────

        private class RetrieveMostActivePlayersDelegate(int seasonID)
            : DataReaderDelegate<IReadOnlyList<MostActivePlayer>>(
                "Basketball.RetrieveMostActivePlayers")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("SeasonID", seasonID);
            }

            public override IReadOnlyList<MostActivePlayer> Translate(
                Command command, IDataRowReader reader)
            {
                var results = new List<MostActivePlayer>();
                while (reader.Read())
                    results.Add(TranslateMostActivePlayer(reader));
                return results;
            }
        }

        private class RetrieveTopScorersByTeamDelegate(int seasonID)
            : DataReaderDelegate<IReadOnlyList<TopScorer>>(
                "Basketball.RetrieveTopScorersByTeam")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("SeasonID", seasonID);
            }

            public override IReadOnlyList<TopScorer> Translate(
                Command command, IDataRowReader reader)
            {
                var results = new List<TopScorer>();
                while (reader.Read())
                    results.Add(TranslateTopScorer(reader));
                return results;
            }
        }

        private class RetrieveTeamPerformanceDelegate(int seasonID)
            : DataReaderDelegate<IReadOnlyList<TeamPerformance>>(
                "Basketball.RetrieveTeamPerformance")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("SeasonID", seasonID);
            }

            public override IReadOnlyList<TeamPerformance> Translate(
                Command command, IDataRowReader reader)
            {
                var results = new List<TeamPerformance>();
                while (reader.Read())
                    results.Add(TranslateTeamPerformance(reader));
                return results;
            }
        }

        private class RetrieveGameStatsSummaryDelegate(
            DateOnly startDate, DateOnly endDate)
            : DataReaderDelegate<IReadOnlyList<GameStatsSummary>>(
                "Basketball.RetrieveGameStatsSummary")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("StartDate",
                    startDate.ToDateTime(TimeOnly.MinValue));
                command.Parameters.AddWithValue("EndDate",
                    endDate.ToDateTime(TimeOnly.MinValue));
            }

            public override IReadOnlyList<GameStatsSummary> Translate(
                Command command, IDataRowReader reader)
            {
                var results = new List<GameStatsSummary>();
                while (reader.Read())
                    results.Add(TranslateGameStatsSummary(reader));
                return results;
            }
        }

        // ── Shared Translators ──────────────────────────────────

        private static MostActivePlayer TranslateMostActivePlayer(IDataRowReader reader)
        {
            return new MostActivePlayer(
                reader.GetInt32("PlayerID"),
                reader.GetString("PlayerName"),
                reader.GetInt32("TeamID"),
                reader.GetInt32("GamesPlayed"),
                reader.GetInt32("TotalPoints"),
                reader.GetValue<decimal>("PointsPerGame"),
                reader.GetInt32("SeasonRank")
            );
        }

        private static TopScorer TranslateTopScorer(IDataRowReader reader)
        {
            return new TopScorer(
                reader.GetInt32("TeamID"),
                reader.GetInt32("PlayerID"),
                reader.GetString("PlayerName"),
                reader.GetInt32("GamesPlayed"),
                reader.GetInt32("TotalPoints"),
                reader.GetValue<decimal>("AveragePointsPerGame"),
                reader.GetInt32("TeamRank")
            );
        }

        private static TeamPerformance TranslateTeamPerformance(IDataRowReader reader)
        {
            return new TeamPerformance(
                reader.GetInt32("TeamID"),
                reader.GetString("TeamName"),
                reader.GetInt32("Wins"),
                reader.GetInt32("Losses"),
                reader.GetValue<decimal>("AverageScorePerGame")
            );
        }

        private static GameStatsSummary TranslateGameStatsSummary(IDataRowReader reader)
        {
            return new GameStatsSummary(
                reader.GetInt32("GameID"),
                DateOnly.FromDateTime(reader.GetDateTime("GameDate",
                    DateTimeKind.Unspecified)),
                reader.GetString("HomeTeam"),
                reader.GetString("AwayTeam"),
                reader.GetValue<decimal>("AveragePoints"),
                reader.GetValue<decimal>("AverageRebounds"),
                reader.GetValue<decimal>("AverageAssists"),
                reader.GetValue<decimal>("AverageTurnovers")
            );
        }
    }
}