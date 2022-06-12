using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication6.Models;
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
            Client client = null;
            try
            {
                client = await _dbService.GetClientIfExists(someSortOfClientTrip.Pesel);
                if (client == null){
                    client = await _dbService.AddClient(someSortOfClientTrip.FirstName, someSortOfClientTrip.LastName, someSortOfClientTrip.Email, someSortOfClientTrip.Telephone, someSortOfClientTrip.Pesel);
                }
                if (!await _dbService.DoesTripExist(id, someSortOfClientTrip.TripName))
                {
                    return NotFound("Wycieczka nie istnieje");
                }

                if (!await _dbService.DoesClientHasTrip(client, id))
                {
                    return Conflict("Klient jest już zapisany na tę wycieczkę.");
                }
                var response = await _dbService.AddClientTrip(client.IdClient, id, someSortOfClientTrip.PaymentDate);
                return Ok(response);
            }catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
