using System;
using System.Collections.Generic;

namespace WebApplication6.Models.DTO
{
    public class SomeSortofTrip
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public IEnumerable<SomeSortOfCountry> Countries { get; set; }
        public IEnumerable<SomeSortOfClient> CLients { get; set; }
    }
}
