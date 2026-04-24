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




    public class SqlLocationRepository(SqlCommandExecutor executor)
        : ILocationRepository
    {
        private readonly SqlCommandExecutor executor = executor;

        public Location CreateLocation(string city, string state, string country)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(city);
            ArgumentException.ThrowIfNullOrWhiteSpace(state);
            ArgumentException.ThrowIfNullOrWhiteSpace(country);

            return executor.ExecuteNonQuery(
                new CreateLocationDelegate(city, state, country));
        }

        public Location FetchLocation(int locationID)
        {
            return executor.ExecuteReader(
                new FetchLocationDelegate(locationID))
                ?? throw new RecordNotFoundException(locationID.ToString());
        }

        public IReadOnlyList<Location> RetrieveLocations()
        {
            return executor.ExecuteReader(
                new RetrieveLocationsDelegate());
        }

        public Location UpdateLocation(int locationID, string city,
            string state, string country)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(city);
            ArgumentException.ThrowIfNullOrWhiteSpace(state);
            ArgumentException.ThrowIfNullOrWhiteSpace(country);

            return executor.ExecuteReader(
                new UpdateLocationDelegate(locationID, city, state, country))
                ?? throw new RecordNotFoundException(locationID.ToString());
        }










        // ── Delegates ──────────────────────────────────────────

        private class CreateLocationDelegate(string city, string state, string country)
            : NonQueryDataDelegate<Location>("Basketball.CreateLocation")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("City", city);
                command.Parameters.AddWithValue("State", state);
                command.Parameters.AddWithValue("Country", country);
                var p = command.Parameters.Add("LocationID", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
            }

            public override Location Translate(Command command)
            {
                var locationID = command.GetParameterValue<int>("LocationID");
                return new Location(locationID, city, state, country);
            }
        }

        private class FetchLocationDelegate(int locationID)
            : DataReaderDelegate<Location?>("Basketball.FetchLocation")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("LocationID", locationID);
            }

            public override Location? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateLocation(reader);
            }
        }

        private class RetrieveLocationsDelegate()
            : DataReaderDelegate<IReadOnlyList<Location>>("Basketball.RetrieveLocations")
        {
            public override IReadOnlyList<Location> Translate(
                Command command, IDataRowReader reader)
            {
                var locations = new List<Location>();
                while (reader.Read())
                    locations.Add(TranslateLocation(reader));
                return locations;
            }
        }

        private class UpdateLocationDelegate(int locationID,
            string city, string state, string country)
            : DataReaderDelegate<Location?>("Basketball.UpdateLocation")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("LocationID", locationID);
                command.Parameters.AddWithValue("City", city);
                command.Parameters.AddWithValue("State", state);
                command.Parameters.AddWithValue("Country", country);
            }

            public override Location? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read())
                    return null;
                return TranslateLocation(reader);
            }
        }






        // ── Shared Translator ───────────────────────────────────

        private static Location TranslateLocation(IDataRowReader reader)
        {
            return new Location(
                reader.GetInt32("LocationID"),
                reader.GetString("City"),
                reader.GetString("State"),
                reader.GetString("Country")
            );
        }
    }
}