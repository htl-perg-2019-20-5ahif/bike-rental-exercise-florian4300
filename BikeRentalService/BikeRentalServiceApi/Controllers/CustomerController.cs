using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static BikeRentalServiceApi.Exceptions;

namespace BikeRentalServiceApi.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {

        private readonly IDataAccess dal;

        public CustomerController(IDataAccess _dal)
        {
            dal = _dal;
            this.dal.InitDatabase();
        }

        [HttpGet]
        public ActionResult GetCustomers([FromQuery] string filter)
        {
            using (dal)
            {
                var customers = dal.GetCustomers(filter);
                return Ok(customers);

            }
        }
        [HttpPost]
        public async Task<ActionResult<int>> AddCustomer([FromBody] ApiCustomer customer)
        {
            using (dal)
            {
                try
                {
                    Customer c = new Customer();
                    c.Birthday = customer.Birthday;
                    c.Firstname = customer.Firstname;
                    c.Lastname = customer.Lastname;
                    c.HouseNumber = customer.HouseNumber;
                    c.Street = customer.Street;
                    c.Town = customer.Town;
                    c.ZipCode = customer.ZipCode;

                    var id = await dal.AddCustomer(c);
                    return Ok(id);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }
        [HttpPut]
        [Route("{customerId}")]
        public async Task<ActionResult<int>> UpdateCustomer(int customerId, [FromBody] ApiCustomer customer)
        {
            using (dal)
            {
                try
                {
                    Customer c = new Customer();
                    c.Gender = customer.Gender;
                    c.Birthday = customer.Birthday;
                    c.Firstname = customer.Firstname;
                    c.Lastname = customer.Lastname;
                    c.HouseNumber = customer.HouseNumber;
                    c.Street = customer.Street;
                    c.Town = customer.Town;
                    c.ZipCode = customer.ZipCode;
                    var updatedCustomerId = await dal.UpdateCustomer(customerId, c);
                    if (updatedCustomerId <= 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(updatedCustomerId);
                    }
                }catch(CustomerNotExistingException ex)
                {
                    return BadRequest();
                }

            }
            //var customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            //if (customerFromDb == null)
            //{
            //    return BadRequest();
            //}
            //customerFromDb.Birthday = customer.Birthday;
            //customerFromDb.Firstname = customer.Firstname;
            //customerFromDb.Lastname = customer.Lastname;
            //customerFromDb.HouseNumber = customer.HouseNumber;
            //customerFromDb.Street = customer.Street;
            //customerFromDb.Town = customer.Town;
            //customerFromDb.ZipCode = customer.ZipCode;
            //context.Customers.Update(customerFromDb);
            //await context.SaveChangesAsync();
            //return Ok(customerFromDb);
        }
        [HttpDelete]
        [Route("{customerId}")]
        public async Task<ActionResult<int>> DeleteCustomer(int customerId)
        {
            using (dal)
            {
                try
                {
                    var deletedCustomer = await dal.DeleteCustomer(customerId);
                    if (deletedCustomer <= 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(deletedCustomer);
                    }
                } catch(CustomerNotExistingException ex)
                {
                    return BadRequest();
                }

            }
            //if (context.Customers.ToList().Find(c => c.CustomerId == customerId) == null)
            //{
            //    return BadRequest();
            //}
            //var c = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            //context.Customers.Remove(c);
            //await context.SaveChangesAsync();
            //return Ok(c);
        }

        [HttpGet]
        [Route("{customerId}/rentals")]
        public ActionResult GetRentalsOfCustomer(int customerId)
        {
            using (dal)
            {
                try
                {
                    var rentals = dal.GetRentalsOfCustomer(customerId);
                    return Ok(rentals);
                }
                catch (CustomerNotExistingException)
                {
                    return BadRequest();
                }

            }

            //if (context.Customers.ToList().Find(c => c.CustomerId == customerId) == null)
            //{
            //    return BadRequest();
            //}
            //var c = context.Customers.ToList().Find(c => c.CustomerId == customerId);

            //return Ok(c.Rentals);
        }

    }
}
