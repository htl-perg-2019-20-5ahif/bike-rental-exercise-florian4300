using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BikeRentalServiceApi.Exceptions;

namespace BikeRentalServiceApi.Controllers
{
    [Route("api/bikes")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly IDataAccess dal;

        public BikeController(IDataAccess _dal)
        {
            dal = _dal;
            dal.InitDatabase();
        }
        // get all bikes with an optional filter
        [HttpGet]
        public ActionResult<List<Bike>> GetBikes([FromQuery] string filter)
        {
            using (dal)
            {
                if (filter == null)
                {
                    filter = "";
                }
                List<Bike> bikes = dal.GetBikes(filter);
                return Ok(bikes);
            }
        }

        // POST: api/Bikes
        // add a Bike
        [HttpPost]
        public async Task<ActionResult<int>> AddBike([FromBody] Bike bike)

        {
            using (dal)
            {
                int bikeId = await dal.AddBike(bike);
                if (bikeId <= 0)
                {
                    return BadRequest();
                }
                return Ok(bikeId);
            }
        }

        // PUT: api/Bikes/5
        // update a specific identified with an Id
        [HttpPut("{bikeId}")]
        public async Task<ActionResult<int>> UpdateBike(int bikeId, [FromBody] Bike bike)
        {
            using (dal)
            {
                try
                {
                    int resultBikeId = await dal.UpdateBike(bikeId, bike);
                    if (resultBikeId <= 0)
                    {
                        return BadRequest();
                    }
                    return Ok(resultBikeId);
                }
                catch (BikeNotExistingException)
                {
                    return BadRequest();
                }

            }

        }

        // DELETE: api/Bikes/5
        // Delete a Bike specified with an Id
        [HttpDelete("{bikeId}")]
        public async Task<ActionResult<int>> DeleteBike(int bikeId)
        {
            using (dal)
            {
                try
                {
                    int id = await dal.DeleteBike(bikeId);
                    return Ok(id);
                }
                catch (BikeNotExistingException)
                {
                    return BadRequest();
                }
                catch (BikeInRentalException)
                {
                    return BadRequest();
                }

            }
        }
    }
}
