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
    public class SqlGameRepository(SqlCommandExecutor executor)
        : IGameRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public Game CreateGame(int homeTeamID, int awayTeamID, int homeTeamScore,
            int awayTeamScore, int courtNumber, int overtimeCount, DateOnly date)
        {
            return executor.ExecuteNonQuery(
                new CreateGameDelegate(homeTeamID, awayTeamID, homeTeamScore,
                    awayTeamScore, courtNumber, overtimeCount, date));
        }

        public Game FetchGame(int gameID)
        {
            return executor.ExecuteReader(
                new FetchGameDelegate(gameID))
                ?? throw new RecordNotFoundException(gameID.ToString());
        }

        public IReadOnlyList<Game> RetrieveGamesByTeam(int teamID)
        {
            return executor.ExecuteReader(
                new RetrieveGamesByTeamDelegate(teamID));
        }

        public Game UpdateGame(int gameID, int homeTeamScore,
            int awayTeamScore, int overtimeCount)
        {
            return executor.ExecuteReader(
                new UpdateGameDelegate(gameID, homeTeamScore,
                    awayTeamScore, overtimeCount))
                ?? throw new RecordNotFoundException(gameID.ToString());
        }

        // ── Delegates ──────────────────────────────────────────

        private class CreateGameDelegate(int homeTeamID, int awayTeamID,
            int homeTeamScore, int awayTeamScore, int courtNumber,
            int overtimeCount, DateOnly date)
            : NonQueryDataDelegate<Game>("Basketball.CreateGame")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("HomeTeamID", homeTeamID);
                command.Parameters.AddWithValue("AwayTeamID", awayTeamID);
                command.Parameters.AddWithValue("HomeTeamScore", homeTeamScore);
                command.Parameters.AddWithValue("AwayTeamScore", awayTeamScore);
                command.Parameters.AddWithValue("CourtNumber", courtNumber);
                command.Parameters.AddWithValue("OvertimeCount", overtimeCount);
                command.Parameters.AddWithValue("Date",
                    date.ToDateTime(TimeOnly.MinValue));
                var p = command.Parameters.Add("GameID", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
            }

            public override Game Translate(Command command)
            {
                var gameID = command.GetParameterValue<int>("GameID");
                return new Game(gameID, homeTeamID, awayTeamID, homeTeamScore,
                    awayTeamScore, courtNumber, overtimeCount, date);
            }
        }

        private class FetchGameDelegate(int gameID)
            : DataReaderDelegate<Game?>("Basketball.FetchGame")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("GameID", gameID);
            }

            public override Game? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateGame(reader);
            }
        }

        private class RetrieveGamesByTeamDelegate(int teamID)
            : DataReaderDelegate<IReadOnlyList<Game>>(
                "Basketball.RetrieveGamesByTeam")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("TeamID", teamID);
            }

            public override IReadOnlyList<Game> Translate(
                Command command, IDataRowReader reader)
            {
                var games = new List<Game>();
                while (reader.Read())
                    games.Add(TranslateGame(reader));
                return games;
            }
        }

        private class UpdateGameDelegate(int gameID, int homeTeamScore,
            int awayTeamScore, int overtimeCount)
            : DataReaderDelegate<Game?>("Basketball.UpdateGame")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("GameID", gameID);
                command.Parameters.AddWithValue("HomeTeamScore", homeTeamScore);
                command.Parameters.AddWithValue("AwayTeamScore", awayTeamScore);
                command.Parameters.AddWithValue("OvertimeCount", overtimeCount);
            }

            public override Game? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateGame(reader);
            }
        }

        // ── Shared Translator ───────────────────────────────────

        private static Game TranslateGame(IDataRowReader reader)
        {
            return new Game(
                reader.GetInt32("GameID"),
                reader.GetInt32("HomeTeamID"),
                reader.GetInt32("AwayTeamID"),
                reader.GetInt32("HomeTeamScore"),
                reader.GetInt32("AwayTeamScore"),
                reader.GetInt32("CourtNumber"),
                reader.GetInt32("OvertimeCount"),
                DateOnly.FromDateTime(reader.GetDateTime("Date",
                    DateTimeKind.Unspecified))
            );
        }
    }
}