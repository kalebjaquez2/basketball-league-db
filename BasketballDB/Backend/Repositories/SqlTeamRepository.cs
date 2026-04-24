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
    public class SqlTeamRepository(SqlCommandExecutor executor)
        : ITeamRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public Team CreateTeam(int seasonID, string teamName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(teamName);

            return executor.ExecuteNonQuery(
                new CreateTeamDelegate(seasonID, teamName));
        }

        public Team FetchTeam(int teamID)
        {
            return executor.ExecuteReader(
                new FetchTeamDelegate(teamID))
                ?? throw new RecordNotFoundException(teamID.ToString());
        }

        public IReadOnlyList<Team> RetrieveTeamsBySeason(int seasonID)
        {
            return executor.ExecuteReader(
                new RetrieveTeamsBySeasonDelegate(seasonID));
        }

        public Team UpdateTeam(int teamID, string teamName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(teamName);

            return executor.ExecuteReader(
                new UpdateTeamDelegate(teamID, teamName))
                ?? throw new RecordNotFoundException(teamID.ToString());
        }

        public void DeleteTeam(int teamID)
        {
            executor.ExecuteNonQuery(
                new DeleteTeamDelegate(teamID));
        }

        // ── Delegates ──────────────────────────────────────────

        private class CreateTeamDelegate(int seasonID, string teamName)
            : NonQueryDataDelegate<Team>("Basketball.CreateTeam")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("SeasonID", seasonID);
                command.Parameters.AddWithValue("TeamName", teamName);
                var p = command.Parameters.Add("TeamID", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
            }

            public override Team Translate(Command command)
            {
                var teamID = command.GetParameterValue<int>("TeamID");
                return new Team(teamID, seasonID, teamName);
            }
        }

        private class FetchTeamDelegate(int teamID)
            : DataReaderDelegate<Team?>("Basketball.FetchTeam")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("TeamID", teamID);
            }

            public override Team? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateTeam(reader);
            }
        }

        private class RetrieveTeamsBySeasonDelegate(int seasonID)
            : DataReaderDelegate<IReadOnlyList<Team>>(
                "Basketball.RetrieveTeamsBySeason")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("SeasonID", seasonID);
            }

            public override IReadOnlyList<Team> Translate(
                Command command, IDataRowReader reader)
            {
                var teams = new List<Team>();
                while (reader.Read())
                    teams.Add(TranslateTeam(reader));
                return teams;
            }
        }

        private class UpdateTeamDelegate(int teamID, string teamName)
            : DataReaderDelegate<Team?>("Basketball.UpdateTeam")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("TeamID", teamID);
                command.Parameters.AddWithValue("TeamName", teamName);
            }

            public override Team? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateTeam(reader);
            }
        }

        private class DeleteTeamDelegate(int teamID)
            : DataDelegate("Basketball.DeleteTeam")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("TeamID", teamID);
            }
        }

        // ── Shared Translator ───────────────────────────────────

        private static Team TranslateTeam(IDataRowReader reader)
        {
            return new Team(
                reader.GetInt32("TeamID"),
                reader.GetInt32("SeasonID"),
                reader.GetString("TeamName")
            );
        }
    }
}