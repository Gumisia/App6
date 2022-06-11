using System.Collections.Generic;

namespace WebApplication6.Models.DTO
{
    public class SomeSortOfClient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<SomeSortOfClientTrip> ClientTrips { get; set; }

    }
}
