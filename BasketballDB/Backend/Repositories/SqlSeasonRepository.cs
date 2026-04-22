using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Backend.Models;
using System.Data;

namespace Backend.Repositories
{
    public class SqlSeasonRepository(SqlCommandExecutor executor)
        : ISeasonRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public Season CreateSeason(int leagueID, DateOnly startDate, DateOnly endDate)
        {
            return executor.ExecuteNonQuery(
                new CreateSeasonDelegate(leagueID, startDate, endDate));
        }

        public Season FetchSeason(int seasonID)
        {
            return executor.ExecuteReader(
                new FetchSeasonDelegate(seasonID))
                ?? throw new RecordNotFoundException(seasonID.ToString());
        }

        public IReadOnlyList<Season> RetrieveSeasonsByLeague(int leagueID)
        {
            return executor.ExecuteReader(
                new RetrieveSeasonsByLeagueDelegate(leagueID));
        }

        public Season UpdateSeason(int seasonID, DateOnly startDate, DateOnly endDate)
        {
            return executor.ExecuteReader(
                new UpdateSeasonDelegate(seasonID, startDate, endDate))
                ?? throw new RecordNotFoundException(seasonID.ToString());
        }

        // ── Delegates ──────────────────────────────────────────

        private class CreateSeasonDelegate(int leagueID,
            DateOnly startDate, DateOnly endDate)
            : NonQueryDataDelegate<Season>("Basketball.CreateSeason")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("LeagueID", leagueID);
                command.Parameters.AddWithValue("StartDate", startDate.ToDateTime(TimeOnly.MinValue));
                command.Parameters.AddWithValue("EndDate", endDate.ToDateTime(TimeOnly.MinValue));
                var p = command.Parameters.Add("SeasonID", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
            }

            public override Season Translate(Command command)
            {
                var seasonID = command.GetParameterValue<int>("SeasonID");
                return new Season(seasonID, leagueID, startDate, endDate);
            }
        }

        private class FetchSeasonDelegate(int seasonID)
            : DataReaderDelegate<Season?>("Basketball.FetchSeason")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("SeasonID", seasonID);
            }

            public override Season? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateSeason(reader);
            }
        }

        private class RetrieveSeasonsByLeagueDelegate(int leagueID)
            : DataReaderDelegate<IReadOnlyList<Season>>(
                "Basketball.RetrieveSeasonsByLeague")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("LeagueID", leagueID);
            }

            public override IReadOnlyList<Season> Translate(
                Command command, IDataRowReader reader)
            {
                var seasons = new List<Season>();
                while (reader.Read())
                    seasons.Add(TranslateSeason(reader));
                return seasons;
            }
        }

        private class UpdateSeasonDelegate(int seasonID,
            DateOnly startDate, DateOnly endDate)
            : DataReaderDelegate<Season?>("Basketball.UpdateSeason")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("SeasonID", seasonID);
                command.Parameters.AddWithValue("StartDate", startDate.ToDateTime(TimeOnly.MinValue));
                command.Parameters.AddWithValue("EndDate", endDate.ToDateTime(TimeOnly.MinValue));
            }

            public override Season? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateSeason(reader);
            }
        }

        // ── Shared Translator ───────────────────────────────────

        private static Season TranslateSeason(IDataRowReader reader)
        {
            return new Season(
                reader.GetInt32("SeasonID"),
                reader.GetInt32("LeagueID"),
                DateOnly.FromDateTime(reader.GetDateTime("StartDate", DateTimeKind.Unspecified)),
                DateOnly.FromDateTime(reader.GetDateTime("EndDate", DateTimeKind.Unspecified))
            );
        }
    }
}