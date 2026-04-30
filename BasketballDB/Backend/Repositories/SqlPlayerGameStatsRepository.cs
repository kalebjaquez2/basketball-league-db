using Backend.Models;
using DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Backend.Repositories
{
    public class SqlPlayerGameStatsRepository(SqlCommandExecutor executor)
        : IPlayerGameStatsRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public void CreatePlayerGameStats(int playerID, int gameID, int teamID,
            int playingTime, int turnovers, int rebounds, int assists,
            int steals, int blocks, int fieldGoalsMade, int fieldGoalsTaken,
            int threePointersMade, int threePointersTaken, int personalFouls)
        {
            executor.ExecuteNonQuery(
                new CreatePlayerGameStatsDelegate(playerID, gameID, teamID,
                    playingTime, turnovers, rebounds, assists, steals, blocks,
                    fieldGoalsMade, fieldGoalsTaken, threePointersMade, threePointersTaken, personalFouls));
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

        public PlayerGameStats UpdatePlayerGameStats(int playerID, int gameID, int teamID,
            int playingTime, int turnovers, int rebounds, int assists,
            int steals, int blocks, int fieldGoalsMade, int fieldGoalsTaken,
            int threePointersMade, int threePointersTaken, int personalFouls)
        {
            return executor.ExecuteReader(
                new UpdatePlayerGameStatsDelegate(playerID, gameID, teamID,
                    playingTime, turnovers, rebounds, assists, steals, blocks,
                    fieldGoalsMade, fieldGoalsTaken, threePointersMade, threePointersTaken, personalFouls))
                ?? throw new RecordNotFoundException($"{playerID},{gameID}");
        }

        public void DeletePlayerGameStats(int playerID, int gameID)
        {
            executor.ExecuteNonQuery(
                new DeletePlayerGameStatsDelegate(playerID, gameID));
        }

        // ── Delegates ──────────────────────────────────────────

        // UPDATED: Primary Constructor now includes all 15 parameters
        private class CreatePlayerGameStatsDelegate(int playerID, int gameID,
            int teamID, int playingTime, int turnovers,
            int rebounds, int assists, int steals, int blocks,
            int fieldGoalsMade, int fieldGoalsTaken, int threePointersMade,
            int threePointersTaken, int personalFouls)
            : DataDelegate("Basketball.CreatePlayerGameStats")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
                command.Parameters.AddWithValue("GameID", gameID);
                command.Parameters.AddWithValue("TeamID", teamID);
                command.Parameters.AddWithValue("PlayingTime", playingTime);
                command.Parameters.AddWithValue("Turnovers", turnovers);
                command.Parameters.AddWithValue("Rebounds", rebounds);
                command.Parameters.AddWithValue("Assists", assists);
                command.Parameters.AddWithValue("Steals", steals);
                command.Parameters.AddWithValue("Blocks", blocks);
                command.Parameters.AddWithValue("FieldGoalsMade", fieldGoalsMade);
                command.Parameters.AddWithValue("FieldGoalsTaken", fieldGoalsTaken);
                command.Parameters.AddWithValue("ThreePointersMade", threePointersMade);
                command.Parameters.AddWithValue("ThreePointersTaken", threePointersTaken);
                command.Parameters.AddWithValue("PersonalFouls", personalFouls);
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
                if (!reader.Read()) return null;
                return TranslateStats(reader);
            }
        }

        private class RetrieveStatsByGameDelegate(int gameID)
            : DataReaderDelegate<IReadOnlyList<PlayerGameStats>>("Basketball.RetrieveStatsByGame")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("GameID", gameID);
            }

            public override IReadOnlyList<PlayerGameStats> Translate(Command command, IDataRowReader reader)
            {
                var stats = new List<PlayerGameStats>();
                while (reader.Read()) stats.Add(TranslateStats(reader));
                return stats;
            }
        }

        private class RetrieveStatsByPlayerDelegate(int playerID)
            : DataReaderDelegate<IReadOnlyList<PlayerGameStats>>("Basketball.RetrieveStatsByPlayer")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
            }

            public override IReadOnlyList<PlayerGameStats> Translate(Command command, IDataRowReader reader)
            {
                var stats = new List<PlayerGameStats>();
                while (reader.Read()) stats.Add(TranslateStats(reader));
                return stats;
            }
        }

        // UPDATED: Constructor and PrepareCommand now include all parameters
        private class UpdatePlayerGameStatsDelegate(int playerID, int gameID, int teamID,
            int playingTime, int turnovers, int rebounds, int assists,
            int steals, int blocks, int fieldGoalsMade, int fieldGoalsTaken,
            int threePointersMade, int threePointersTaken, int personalFouls)
            : DataReaderDelegate<PlayerGameStats?>("Basketball.UpdatePlayerGameStats")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
                command.Parameters.AddWithValue("GameID", gameID);
                command.Parameters.AddWithValue("TeamID", teamID);
                command.Parameters.AddWithValue("PlayingTime", playingTime);
                command.Parameters.AddWithValue("Turnovers", turnovers);
                command.Parameters.AddWithValue("Rebounds", rebounds);
                command.Parameters.AddWithValue("Assists", assists);
                command.Parameters.AddWithValue("Steals", steals);
                command.Parameters.AddWithValue("Blocks", blocks);
                command.Parameters.AddWithValue("FieldGoalsMade", fieldGoalsMade);
                command.Parameters.AddWithValue("FieldGoalsTaken", fieldGoalsTaken);
                command.Parameters.AddWithValue("ThreePointersMade", threePointersMade);
                command.Parameters.AddWithValue("ThreePointersTaken", threePointersTaken);
                command.Parameters.AddWithValue("PersonalFouls", personalFouls);
            }

            public override PlayerGameStats? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read()) return null;
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

        private static PlayerGameStats TranslateStats(IDataRowReader reader)
        {
            return new PlayerGameStats(
                reader.GetInt32("PlayerID"),
                reader.GetInt32("GameID"),
                reader.GetInt32("TeamID"),
                reader.GetInt32("PlayingTime"),
                reader.GetInt32("Turnovers"),
                reader.GetInt32("Rebounds"),
                reader.GetInt32("Assists"),
                reader.GetInt32("Steals"),
                reader.GetInt32("Blocks"),
                reader.GetInt32("FieldGoalsTaken"),
                reader.GetInt32("FieldGoalsMade"),
                reader.GetInt32("ThreePointersTaken"),
                reader.GetInt32("ThreePointersMade"),
                reader.GetInt32("PersonalFouls")
            )
            {
                PlayerName = reader.GetString("PlayerName")
            };
        }
    }
}