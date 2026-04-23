using DataAccess;
using Backend.Models;
using System.Data;

namespace Backend.Repositories
{
    public class SqlLeagueRepository(SqlCommandExecutor executor)
        : ILeagueRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public League CreateLeague(string leagueName, int locationId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(leagueName);

            return executor.ExecuteNonQuery(
                new CreateLeagueDelegate(leagueName, locationId)); // Pass int here
        }

        public League FetchLeague(int leagueID)
        {
            return executor.ExecuteReader(
                new FetchLeagueDelegate(leagueID))
                ?? throw new RecordNotFoundException(leagueID.ToString());
        }

        public IReadOnlyList<League> RetrieveLeagues()
        {
            return executor.ExecuteReader(
                new RetrieveLeaguesDelegate());
        }

        public League UpdateLeague(int leagueID, string leagueName, int locationId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(leagueName);

            return executor.ExecuteReader(
                new UpdateLeagueDelegate(leagueID, leagueName, locationId))
                ?? throw new RecordNotFoundException(leagueID.ToString());
        }






        // ── Delegates ──────────────────────────────────────────

        // Update these parameters to take 'int locationId'
        private class CreateLeagueDelegate(string leagueName, int locationId)
            : NonQueryDataDelegate<League>("Basketball.CreateLeague")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("LeagueName", leagueName);
                // This MUST match the @LocationID name in your Stored Procedure
                command.Parameters.AddWithValue("LocationID", locationId);

                var p = command.Parameters.Add("LeagueID", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
            }

            public override League Translate(Command command)
            {
                var leagueID = command.GetParameterValue<int>("LeagueID");
                // Pass locationId as the 4th argument
                return new League(leagueID, leagueName, "Location Linked", locationId);
      
            }
        }

        private class FetchLeagueDelegate(int leagueID)
            : DataReaderDelegate<League?>("Basketball.FetchLeague")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("LeagueID", leagueID);
            }

            public override League? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateLeague(reader);
            }
        }

        private class RetrieveLeaguesDelegate()
            : DataReaderDelegate<IReadOnlyList<League>>("Basketball.RetrieveLeagues")
        {
            public override IReadOnlyList<League> Translate(
                Command command, IDataRowReader reader)
            {
                var leagues = new List<League>();
                while (reader.Read())
                    leagues.Add(TranslateLeague(reader));
                return leagues;
            }
        }

        private class UpdateLeagueDelegate(int leagueID, string leagueName, int locationId) // Change string to int
           : DataReaderDelegate<League?>("Basketball.UpdateLeague")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("LeagueID", leagueID);
                command.Parameters.AddWithValue("LeagueName", leagueName);
                command.Parameters.AddWithValue("LocationID", locationId); // Use LocationID
            }

            public override League? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateLeague(reader);
            }
        }










        // ── Shared Translator ───────────────────────────────────

        private static League TranslateLeague(IDataRowReader reader)
        {
            return new League(
                reader.GetInt32("LeagueID"),
                reader.GetString("LeagueName"),
                reader.GetString("Location"),   // This is the City/State string from the JOIN
                reader.GetInt32("LocationID")  // Add this to satisfy the 4th parameter
            );
        }
    }
}