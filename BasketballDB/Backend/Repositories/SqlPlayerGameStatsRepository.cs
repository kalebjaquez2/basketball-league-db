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
    public class SqlPlayerGameStatsRepository(SqlCommandExecutor executor)
        : IPlayerGameStatsRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public void CreatePlayerGameStats(int playerID, int gameID, int teamID,
            int points, int playingTime, int turnovers, int rebounds, int assists)
        {
            executor.ExecuteNonQuery(
                new CreatePlayerGameStatsDelegate(playerID, gameID, teamID,
                    points, playingTime, turnovers, rebounds, assists));
        }

        public PlayerGameStats FetchPlayerGameStats(int playerID, int gameID)
        {
            return executor.ExecuteReader(
                new FetchPlayerGameStatsDelegate(playerID, gameID))
                ?? throw new RecordNotFoundException($"{playerID},{gameID}");
        }

        public IReadOnlyList<PlayerGameStats> RetrieveStatsByGame(int gameID)
        {
            return executor.ExecuteReader(
                new RetrieveStatsByGameDelegate(gameID));
        }

        public IReadOnlyList<PlayerGameStats> RetrieveStatsByPlayer(int playerID)
        {
            return executor.ExecuteReader(
                new RetrieveStatsByPlayerDelegate(playerID));
        }

        public PlayerGameStats UpdatePlayerGameStats(int playerID, int gameID,
            int points, int playingTime, int turnovers, int rebounds, int assists)
        {
            return executor.ExecuteReader(
                new UpdatePlayerGameStatsDelegate(playerID, gameID,
                    points, playingTime, turnovers, rebounds, assists))
                ?? throw new RecordNotFoundException($"{playerID},{gameID}");
        }

        public void DeletePlayerGameStats(int playerID, int gameID)
        {
            executor.ExecuteNonQuery(
                new DeletePlayerGameStatsDelegate(playerID, gameID));
        }

        // ── Delegates ──────────────────────────────────────────

        private class CreatePlayerGameStatsDelegate(int playerID, int gameID,
            int teamID, int points, int playingTime, int turnovers,
            int rebounds, int assists)
            : DataDelegate("Basketball.CreatePlayerGameStats")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
                command.Parameters.AddWithValue("GameID", gameID);
                command.Parameters.AddWithValue("TeamID", teamID);
                command.Parameters.AddWithValue("Points", points);
                command.Parameters.AddWithValue("PlayingTime", playingTime);
                command.Parameters.AddWithValue("Turnovers", turnovers);
                command.Parameters.AddWithValue("Rebounds", rebounds);
                command.Parameters.AddWithValue("Assists", assists);
            }
        }

        private class FetchPlayerGameStatsDelegate(int playerID, int gameID)
            : DataReaderDelegate<PlayerGameStats?>("Basketball.FetchPlayerGameStats")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
                command.Parameters.AddWithValue("GameID", gameID);
            }

            public override PlayerGameStats? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateStats(reader);
            }
        }

        private class RetrieveStatsByGameDelegate(int gameID)
            : DataReaderDelegate<IReadOnlyList<PlayerGameStats>>(
                "Basketball.RetrieveStatsByGame")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("GameID", gameID);
            }

            public override IReadOnlyList<PlayerGameStats> Translate(
                Command command, IDataRowReader reader)
            {
                var stats = new List<PlayerGameStats>();
                while (reader.Read())
                    stats.Add(TranslateStats(reader));
                return stats;
            }
        }

        private class RetrieveStatsByPlayerDelegate(int playerID)
            : DataReaderDelegate<IReadOnlyList<PlayerGameStats>>(
                "Basketball.RetrieveStatsByPlayer")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
            }

            public override IReadOnlyList<PlayerGameStats> Translate(
                Command command, IDataRowReader reader)
            {
                var stats = new List<PlayerGameStats>();
                while (reader.Read())
                    stats.Add(TranslateStats(reader));
                return stats;
            }
        }

        private class UpdatePlayerGameStatsDelegate(int playerID, int gameID,
            int points, int playingTime, int turnovers, int rebounds, int assists)
            : DataReaderDelegate<PlayerGameStats?>("Basketball.UpdatePlayerGameStats")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
                command.Parameters.AddWithValue("GameID", gameID);
                command.Parameters.AddWithValue("Points", points);
                command.Parameters.AddWithValue("PlayingTime", playingTime);
                command.Parameters.AddWithValue("Turnovers", turnovers);
                command.Parameters.AddWithValue("Rebounds", rebounds);
                command.Parameters.AddWithValue("Assists", assists);
            }

            public override PlayerGameStats? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateStats(reader);
            }
        }

        private class DeletePlayerGameStatsDelegate(int playerID, int gameID)
            : DataDelegate("Basketball.DeletePlayerGameStats")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
                command.Parameters.AddWithValue("GameID", gameID);
            }
        }

        // ── Shared Translator ───────────────────────────────────

        private static PlayerGameStats TranslateStats(IDataRowReader reader)
        {
            return new PlayerGameStats(
                reader.GetInt32("PlayerID"),
                reader.GetInt32("GameID"),
                reader.GetInt32("TeamID"),
                reader.GetInt32("Points"),
                reader.GetInt32("PlayingTime"),
                reader.GetInt32("Turnovers"),
                reader.GetInt32("Rebounds"),
                reader.GetInt32("Assists")
            );
        }
    }
}