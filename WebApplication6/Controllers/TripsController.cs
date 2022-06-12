using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication6.Models.DTO;
using WebApplication6.Services;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public TripsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _dbService.GetTrips();
            return Ok(trips);
        }

        /*[HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTrips(int id)
        {
            await _dbService.RemoveTrip(id);
            return Ok("Removed Trip");

        }*/

        [HttpPost]
        [Route("{id}/clients")]

        public async Task<IActionResult> PostClientTrip(int id, SomeSortOfClientTrip someSortOfClientTrip)
        {
            try
            {
                if (!await _dbService.DoesKlientExist(someSortOfClientTrip.Pesel)){
                    await _dbService.AddClient(someSortOfClientTrip.FirstName, someSortOfClientTrip.LastName, someSortOfClientTrip.Email, someSortOfClientTrip.Telephone, someSortOfClientTrip.Pesel);
                }

                if (!await _dbService.DoesClientHasTrip(someSortOfClientTrip.FirstName, someSortOfClientTrip.LastName, id)) return Conflict("Klient ma tę wycieczkę");
                if (!await _dbService.DoesTripExist(id, someSortOfClientTrip.TripName)) return NotFound("Wycieczka nie istnieje");

                await _dbService.AddClientTrip(id, someSortOfClientTrip);
                return Ok("Client trip added");
            }catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
