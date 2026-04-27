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
    public class SqlPlayerRepository(SqlCommandExecutor executor)
        : IPlayerRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public Player CreatePlayer(int gameID, int jerseyNumber, string firstName,
            string lastName, int? age, string? height, int? weight)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

            return executor.ExecuteNonQuery(
                new CreatePlayerDelegate(gameID, jerseyNumber, firstName,
                    lastName, age, height, weight));
        }

        public Player FetchPlayer(int playerID)
        {
            return executor.ExecuteReader(
                new FetchPlayerDelegate(playerID))
                ?? throw new RecordNotFoundException(playerID.ToString());
        }

        public IReadOnlyList<Player> RetrievePlayersByGame(int gameID)
        {
            return executor.ExecuteReader(
                new RetrievePlayersByGameDelegate(gameID));
        }

        public Player UpdatePlayer(int playerID, int jerseyNumber,
            int? age, string? height, int? weight)
        {
            return executor.ExecuteReader(
                new UpdatePlayerDelegate(playerID, jerseyNumber, age, height, weight))
                ?? throw new RecordNotFoundException(playerID.ToString());
        }

        public void DeletePlayer(int playerID)
        {
            executor.ExecuteNonQuery(
                new DeletePlayerDelegate(playerID));
        }

        // ── Delegates ──────────────────────────────────────────

        private class CreatePlayerDelegate(int gameID, int jerseyNumber,
            string firstName, string lastName, int? age,
            string? height, int? weight)
            : NonQueryDataDelegate<Player>("Basketball.CreatePlayer")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("GameID", gameID);
                command.Parameters.AddWithValue("JerseyNumber", jerseyNumber);
                command.Parameters.AddWithValue("FirstName", firstName);
                command.Parameters.AddWithValue("LastName", lastName);
                command.Parameters.AddWithValue("Age",
                    age as object ?? DBNull.Value);
                command.Parameters.AddWithValue("Height",
                    height as object ?? DBNull.Value);
                command.Parameters.AddWithValue("Weight",
                    weight as object ?? DBNull.Value);
                var p = command.Parameters.Add("PlayerID", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
            }

            public override Player Translate(Command command)
            {
                var playerID = command.GetParameterValue<int>("PlayerID");
                return new Player(playerID, gameID, jerseyNumber,
                    firstName, lastName, age, height, weight);
            }
        }

        private class FetchPlayerDelegate(int playerID)
            : DataReaderDelegate<Player?>("Basketball.FetchPlayer")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
            }

            public override Player? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslatePlayer(reader);
            }
        }

        private class RetrievePlayersByGameDelegate(int gameID)
            : DataReaderDelegate<IReadOnlyList<Player>>(
                "Basketball.RetrievePlayersByGame")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("GameID", gameID);
            }

            public override IReadOnlyList<Player> Translate(
                Command command, IDataRowReader reader)
            {
                var players = new List<Player>();
                while (reader.Read())
                    players.Add(TranslatePlayer(reader));
                return players;
            }
        }

        private class UpdatePlayerDelegate(int playerID, int jerseyNumber,
            int? age, string? height, int? weight)
            : DataReaderDelegate<Player?>("Basketball.UpdatePlayer")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
                command.Parameters.AddWithValue("JerseyNumber", jerseyNumber);
                command.Parameters.AddWithValue("Age",
                    age as object ?? DBNull.Value);
                command.Parameters.AddWithValue("Height",
                    height as object ?? DBNull.Value);
                command.Parameters.AddWithValue("Weight",
                    weight as object ?? DBNull.Value);
            }

            public override Player? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslatePlayer(reader);
            }
        }

        private class DeletePlayerDelegate(int playerID)
            : DataDelegate("Basketball.DeletePlayer")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("PlayerID", playerID);
            }
        }

        // ── Shared Translator ───────────────────────────────────

        private static Player TranslatePlayer(IDataRowReader reader)
        {
            return new Player(
                reader.GetInt32("PlayerID"),
                reader.GetInt32("GameID"),
                reader.GetInt32("JerseyNumber"),
                reader.GetString("FirstName"),
                reader.GetString("LastName"),
                reader.GetValue<int?>("Age", null),
                reader.GetValue<string?>("Height", null),
                reader.GetValue<int?>("Weight", null)
            );
        }
    }
}