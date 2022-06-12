using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication6.Models;
using WebApplication6.Models.DTO;

namespace WebApplication6.Services
{
    public interface IDbService
    {
        Task<IEnumerable<SomeSortofTrip>> GetTrips();
        Task RemoveTrip(int id);
        Task<string> RemoveClient(int id);
        Task<string> AddClientTrip(int clientId, int tripId, string paymentDate = "");
        Task<Client> GetClientIfExists(string pesel);
        Task<bool> DoesClientHasTrip(Client client, int tripId);
        Task<bool> DoesTripExist(int id, string tripName);
        Task<Client> AddClient(string firstName, string lastName, string email, string telephone, string pesel);
    }
}
