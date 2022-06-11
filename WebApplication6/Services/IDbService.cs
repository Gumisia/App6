using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication6.Models.DTO;

namespace WebApplication6.Services
{
    public interface IDbService
    {
        Task<IEnumerable<SomeSortofTrip>> GetTrips();
        Task RemoveTrip(int id);
        Task<string> RemoveClient(int id);
        Task<string> AddClientTrip(int id, SomeSortOfClientTrip request);
        Task<bool> DoesKlientExist(string pesel);
        Task<bool> DoesClientHasTrip(string firstName, string lastName, int id);
        Task<bool> DoesTripExist(int id, string tripName);
        Task<bool> AddClient(string firstName, string lastName, string email, string telephone, string pesel);
    }
}
