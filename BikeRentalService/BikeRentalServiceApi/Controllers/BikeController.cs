using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalServiceApi.Controllers
{
    [Route("api/bikes")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly BikeRentalContext context;

        public BikeController(BikeRentalContext _context)
        {
            context = _context;
        }
        // GET: api/Bikes
        [HttpGet]
        public ActionResult GetBikes([FromQuery] string filter)
        {
            List<Bike> availableBikes;
            if (filter.Equals("priceFirstHour"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null).OrderBy(b => b.RentalPriceFirstHour).ToList();
            }
            else if (filter.Equals("priceAdditionalHours"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null).OrderBy(b => b.RentalPriceAdditionalHours).ToList(); ;
            }
            else if (filter.Equals("purchaseData"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null).OrderBy(b => b.RentalPriceAdditionalHours).ToList(); ;
            }
            else
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null);
            }
            return Ok(availableBikes);
        }

        // POST: api/Bikes
        [HttpPost]
        public async Task<ActionResult> AddBike([FromBody] Bike bike)

        {
            await context.Bikes.AddAsync(bike);
            await context.SaveChangesAsync();
            return Ok(bike);
        }

        // PUT: api/Bikes/5
        [HttpPut("{bikeId}")]
        public async Task<ActionResult> UpdateBike(int bikeId, [FromBody] Bike bike)
        {
            Bike bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            if (bikeFromDb == null)
            {
                return BadRequest();
            }

            bikeFromDb.Brand = bike.Brand;
            bikeFromDb.LastServiceDate = bike.LastServiceDate;
            bikeFromDb.Notes = bike.Notes;
            bikeFromDb.PurchaseDate = bike.PurchaseDate;
            bikeFromDb.RentalPriceAdditionalHours = bike.RentalPriceAdditionalHours;
            bikeFromDb.RentalPriceFirstHour = bike.RentalPriceFirstHour;
            context.Update(bikeFromDb);
            await context.SaveChangesAsync();
            return Ok(bikeFromDb);

        }

        // DELETE: api/Bikes/5
        [HttpDelete("{bikeId}")]
        public async Task<ActionResult> Delete(int bikeId)
        {
            if (context.Bikes.ToList().Find(b => b.BikeId == bikeId) == null)
            {
                return BadRequest();
            }
            Bike b = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            context.Bikes.Remove(b);
            await context.SaveChangesAsync();
            return Ok(b);
        }
    }
}
