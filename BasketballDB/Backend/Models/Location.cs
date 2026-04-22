using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Location
    {
        public int LocationID { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }

        public Location(int locationID, string city, string state, string country)
        {
            LocationID = locationID;
            City = city;
            State = state;
            Country = country;
        }
    }
}
