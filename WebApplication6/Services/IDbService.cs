using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication6.Models.DTO;

namespace WebApplication6.Services
{
    public interface IDbService
    {
        Task<IEnumerable<SomeSortofTrip>> GetTrips();
        Task RemoveTrip(int id);
    }
}
