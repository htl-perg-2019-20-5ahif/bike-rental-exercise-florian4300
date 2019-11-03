using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsControllerGenerated : ControllerBase
    {
        private readonly BikeRentalContext context;

        public RentalsControllerGenerated(BikeRentalContext _context)
        {
            context = _context;
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return await context.Rentals.ToListAsync();
        }

        // GET: api/Rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }

        // PUT: api/Rentals/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRental(int id, Rental rental)
        {
            if (id != rental.RentalId)
            {
                return BadRequest();
            }

            context.Entry(rental).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (this.GetRentalById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("start")]
        public async Task<ActionResult<Rental>> StartRental([FromBody] Rental rental)
        {
            rental.RentalBegin = System.DateTime.Now;
            rental.RentalEnd = null;
            context.Rentals.Add(rental);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalId }, rental);
        }
        [HttpPost]
        [Route("stop/{rentalId}")]
        public async Task<ActionResult<Rental>> StopRental(int rentalId)
        {
            var rental = this.GetRentalById(rentalId);
            if (rental == null)
            {
                return NotFound();
            }
            rental.RentalEnd = System.DateTime.Now;
            rental.TotalAmount = CalculateTotalPrice(rental);
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalId }, rental);
        }

        [HttpPost]
        [Route("{rentalId}/pay")]
        public async Task<ActionResult<Rental>> payRental(int rentalId)
        {
            var rental = this.GetRentalById(rentalId);
            if (rental == null)
            {
                return NotFound();
            }
            if (rental.RentalEnd == null)
            {
                return BadRequest();
            }
            if (rental.TotalAmount > 0)
            {
                return BadRequest();
            }
            rental.Paid = true;
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalId }, rental);
        }

        [HttpGet("unpaid")]
        public ActionResult<List<UnpaidRental>> GetUnpaid(int id)
        {
            List<UnpaidRental> unpaidRentals = new List<UnpaidRental>();
            var rentals = context.Rentals;
            foreach (var rental in rentals)
            {
                if (rental.TotalAmount > 0 && rental.RentalEnd != null && rental.Paid == false)
                {
                    UnpaidRental ur = new UnpaidRental();
                    ur.CustomerId = rental.Customer.CustomerId;
                    ur.Firstname = rental.Customer.Firstname;
                    ur.Lastname = rental.Customer.Lastname;
                    ur.RentalId = rental.RentalId;
                    ur.RentalBegin = ur.RentalBegin;
                    ur.RentalEnd = ur.RentalEnd;
                }
            }
            return unpaidRentals;


        }



        private Rental GetRentalById(int id)
        {
            return context.Rentals.Where(e => e.RentalId == id).FirstOrDefault();
        }
        private double CalculateTotalPrice(Rental rental)
        {
            double totalCost = 0.0;
            if (rental.RentalBegin != null && rental.RentalEnd != null)
            {
                TimeSpan ts = (TimeSpan)(rental.RentalBegin - rental.RentalEnd);
                if (ts.TotalMinutes <= 15)
                {
                    return 0;
                }
                ts = ts.Subtract(TimeSpan.FromHours(1));
                totalCost += rental.Bike.RentalPriceFirstHour;
                while (ts.TotalMinutes > 0)
                {
                    ts = ts.Subtract(TimeSpan.FromHours(1));
                    totalCost += rental.Bike.RentalPriceAdditionalHours;
                }

            }

            return totalCost;

        }
    }
}
