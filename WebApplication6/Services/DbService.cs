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

        public async Task<bool> AddClient(string firstName, string lastName, string email, string telephone, string pesel)
        {
            var addClient = new Client
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Telephone = telephone,
                Pesel = pesel
            };
            _dbContext.Add(addClient);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddClientTrip(int id, SomeSortOfClientTrip request)
        {
            using(var tran = await _dbContext.Database.BeginTransactionAsync())
            {
                var trip = await _dbContext.Trips.Where(e => e.IdTrip == id).FirstOrDefaultAsync();
                var newTrip = new SomeSortOfClientTrip
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel,
                    IdTrip = id,
                    TripName = request.TripName,
                    PaymentDate = request.PaymentDate
                };
                _dbContext.Add(newTrip);

                await _dbContext.SaveChangesAsync();
                await tran.CommitAsync();
            }
            return true;
        }

        public async Task<bool> DoesClientHasTrip(string firstName, string lastName, int id)
        {
            var klient = await _dbContext.Clients.FromSqlRaw("" +
                "SELECT * " +
                "FROM Client " +
                "WHERE firstName={0} AND lastName={1}" +
                "AND NOT EXISTS (SELECT 1 FROM Client_Trip WHERE IdClient={2})", firstName, lastName, id).SingleOrDefaultAsync();
            if (klient == null) return true;
            return false;
        }

        public async Task<bool> DoesKlientExist(string pesel)
        {
            var klient = await _dbContext.Clients.Where(e => e.Pesel == pesel).FirstOrDefaultAsync();
            if (klient is null) return false;
            return true;
        }

        public async Task<bool> DoesTripExist(int id, string tripName)
        {
            var _trip = await _dbContext.Trips.Where(e => e.Name == tripName).FirstOrDefaultAsync();
            if (_trip is null) return false;
            return true;
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
            }).OrderByDescending(e => e.DateFrom).ToListAsync();
        }

        public async Task<string> RemoveClient(int id)
        {
            var response = "";
            var client = await _dbContext.Clients.FromSqlRaw("" +
                "SELECT * " +
                "FROM Client " +
                "WHERE IdClient={0} " +
                "AND NOT EXISTS (SELECT 1 FROM Client_Trip WHERE IdClient={0})", id).SingleOrDefaultAsync();
            if (client != null)
            {
                _dbContext.Remove(client);
                 response = "Client Removed";
                await _dbContext.SaveChangesAsync();
            }
            else {
                response = "ClientNotFound";

            }
            //var client = new Client() { IdClient = id };

            //  _dbContext.Attach(client);
            //  await _dbContext.SaveChangesAsync();
            return response;
        }

        public async Task RemoveTrip(int id)
        {
         //   var trip = _dbContext.Trips.Where(e => e.IdTrip == id).FirstOrDefaultAsync();
         //

            var trip = new Trip() { IdTrip = id };

            _dbContext.Attach(trip);
            _dbContext.Remove(trip);
            await _dbContext.SaveChangesAsync(); // dopiero po tym zapisuje do bazy
        }

        Task<string> IDbService.AddClientTrip(int id, SomeSortOfClientTrip request)
        {
            throw new System.NotImplementedException();
        }
    }
}
