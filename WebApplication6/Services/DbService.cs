using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication6.Models.DTO;
using WebApplication6.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Globalization;

namespace WebApplication6.Services
{
    public class DbService : IDbService
    {
        private readonly s17461Context _dbContext;

        public DbService(s17461Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client> AddClient(string firstName, string lastName, string email, string telephone, string pesel)
        {
            var client = new Client
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Telephone = telephone,
                Pesel = pesel
            };
            _dbContext.Add(client);
            await _dbContext.SaveChangesAsync();
            return client;
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

        public async Task<bool> DoesClientHasTrip(Client client, int tripId)
        {
            var klient = await _dbContext.Clients.FromSqlRaw("" +
                "SELECT * " +
                "FROM Client " +
                $"WHERE Client.IdClient = {client.IdClient}" +
                $"AND EXISTS (SELECT 1 FROM Client_Trip WHERE Client_Trip.IdClient={client.IdClient} AND Client_Trip.IdTrip={tripId})").SingleOrDefaultAsync();
            if (klient == null) return false;
            return true;
        }

        public async Task<Client> GetClientIfExists(string pesel)
        {
            var klient = await _dbContext.Clients.Where(e => e.Pesel == pesel).FirstOrDefaultAsync();
            if (klient is null) return null;
            return klient;
        }

        public async Task<bool> DoesTripExist(int id, string tripName)
        {
            var _trip = await _dbContext.Trips.Where(e => e.Name == tripName).Where(e => e.IdTrip == id).FirstOrDefaultAsync();
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

        public async Task<string> AddClientTrip(int clientId, int tripId, string PaymentDate)
        {
            try
            {
                var clientTrip = new ClientTrip
                {
                    IdClient = clientId,
                    IdTrip = tripId,
                    PaymentDate = PaymentDate != "" ? DateTime.ParseExact(PaymentDate, "yyyy/MM/dd", null) : null,
                    RegisteredAt = DateTime.Now
                };

                _dbContext.Add(clientTrip);
                await _dbContext.SaveChangesAsync();

                return "Dodano wycieczke";
            } catch
            {
                return "Nie udało się dodać wycieczki";
            }


        }
    }
}
