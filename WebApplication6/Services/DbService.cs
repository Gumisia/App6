using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication6.Models.DTO;
using WebApplication6.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApplication6.Services
{
    public class DbService : IDbService
    {
        private readonly s17461Context _dbContext;

        public DbService(s17461Context dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<SomeSortofTrip>> GetTrips()
        {
            return await _dbContext.Trips
                .Select(e => new SomeSortofTrip
            {
                Name = e.Name,
                Description = e.Description,
                MaxPeople = e.MaxPeople,
                DateFrom = e.DateFrom,
                DateTo = e.DateTo,
                Countries = e.CountryTrips.Select(e => new SomeSortOfCountry {  Name = e.IdCountryNavigation.Name}).ToList(),
                CLients = e.ClientTrips.Select(e=> new SomeSortOfClient { FirstName = e.IdClientNavigation.FirstName, LastName = e.IdClientNavigation.LastName}).ToList()
            }).ToListAsync();
        }
    }
}
