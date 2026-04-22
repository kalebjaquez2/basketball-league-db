using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;


namespace Backend.Repositories
{
    public interface ILocationRepository
    {
        /// <summary>
        /// Creates a new location in the database.
        /// </summary>
        Location CreateLocation(string city, string state, string country);

        /// <summary>
        /// Fetches a location by ID. Throws RecordNotFoundException if not found.
        /// </summary>
        Location FetchLocation(int locationID);

        /// <summary>
        /// Retrieves all locations in the database.
        /// </summary>
        IReadOnlyList<Location> RetrieveLocations();

        /// <summary>
        /// Updates an existing location.
        /// </summary>
        Location UpdateLocation(int locationID, string city, string state, string country);
    }
}
