using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BikeRentalServiceApi.Exceptions;

namespace BikeRentalServiceApi.Controllers
{
    [Route("api/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDataAccess dal;

        public RentalsController(IDataAccess _dal)
        {
            dal = _dal;
            dal.InitDatabase();
        }

        // POST: api/Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("start")]
        public async Task<ActionResult<Rental>> StartRental([FromBody] CustomerAndBikeId ids)
        {
            using (dal)
            {
                try
                {
                    Rental rental = await dal.StartRental(ids.CustomerId, ids.BikeId);
                    return Ok((RentalApi)rental);
                }
                catch (CustomerNotExistingException)
                {
                    return BadRequest();
                }
                catch (BikeNotExistingException)
                {
                    return BadRequest();
                }
                catch (BikeAlreadyInRentalException)
                {
                    return BadRequest();
                }
                catch (CustomerAlreadyInRentalException)
                {
                    return BadRequest();
                }

            }
        }
        [HttpPost]
        [Route("{rentalId}/stop")]
        public async Task<ActionResult<Rental>> StopRental(int rentalId, [FromBody] DateTime End)
        {
            using (dal)
            {
                try
                {
                    Rental rental = await dal.StopRental(rentalId, End);
                    return Ok((RentalApi)rental);
                }
                catch (RentalNotExistingException)
                {
                    return BadRequest();
                }
                catch (RentalAlreadyEndedException)
                {
                    return BadRequest();
                }
                catch (ArgumentException)
                {
                    return BadRequest();
                }

            }
        }

        [HttpPost]
        [Route("{rentalId}/pay")]
        public async Task<ActionResult<Rental>> payRental(int rentalId)
        {
            using (dal)
            {
                try
                {
                    Rental rental = await dal.payRental(rentalId);
                    return Ok((RentalApi)rental);
                }
                catch (RentalNotExistingException)
                {
                    return BadRequest();
                }
                catch (RentalNotEndedException)
                {
                    return BadRequest();
                }
                catch (TotalAmountNotGreaterThanZeroException)
                {
                    return BadRequest();
                }

            }
        }

        [HttpGet("unpaid")]
        public ActionResult<List<UnpaidRental>> GetUnpaid(int id)
        {
            using (dal)
            {
                List<UnpaidRental> unpaidRentals = dal.GetUnpaid();
                return Ok(unpaidRentals);
            }

        }
    }
}
