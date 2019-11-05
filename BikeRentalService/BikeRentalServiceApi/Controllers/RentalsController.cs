using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this.dal.InitDatabase();
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
                    var rental = await dal.StartRental(ids.CustomerId, ids.BikeId);
                    return Ok((RentalApi) rental);
                } catch(CustomerNotExistingException ex)
                {
                    return BadRequest();
                }
                catch(BikeNotExistingException ex)
                {
                    return BadRequest();
                } catch(BikeAlreadyInRentalException ex)
                {
                    return BadRequest();
                } catch( CustomerAlreadyInRentalException ex)
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
                    var rental = await dal.StopRental(rentalId, End);
                    return Ok((RentalApi)rental);
                } catch (RentalNotExistingException ex)
                {
                    return BadRequest();
                } catch (RentalAlreadyEndedException ex)
                {
                    return BadRequest();
                } catch (ArgumentException)
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
                    var rental = await dal.payRental(rentalId);
                    return Ok((RentalApi) rental);
                }
                catch (RentalNotExistingException ex)
                {
                    return BadRequest();
                }
                catch (RentalNotEndedException ex)
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
                var unpaidRentals = dal.GetUnpaid();
                return Ok(unpaidRentals);
            }

        }
    }
}
