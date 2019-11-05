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
            dal.InitDatabase();
        }

        [HttpGet]
        public ActionResult GetCustomers([FromQuery] string filter)
        {
            using (dal)
            {
                System.Collections.Generic.List<Customer> customers = dal.GetCustomers(filter);
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
                    Customer c = new Customer
                    {
                        Birthday = customer.Birthday,
                        Firstname = customer.Firstname,
                        Lastname = customer.Lastname,
                        HouseNumber = customer.HouseNumber,
                        Street = customer.Street,
                        Town = customer.Town,
                        ZipCode = customer.ZipCode
                    };

                    int id = await dal.AddCustomer(c);
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
                    Customer c = new Customer
                    {
                        Gender = customer.Gender,
                        Birthday = customer.Birthday,
                        Firstname = customer.Firstname,
                        Lastname = customer.Lastname,
                        HouseNumber = customer.HouseNumber,
                        Street = customer.Street,
                        Town = customer.Town,
                        ZipCode = customer.ZipCode
                    };
                    int updatedCustomerId = await dal.UpdateCustomer(customerId, c);
                    if (updatedCustomerId <= 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(updatedCustomerId);
                    }
                }
                catch (CustomerNotExistingException)
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
                    int deletedCustomer = await dal.DeleteCustomer(customerId);
                    if (deletedCustomer <= 0)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(deletedCustomer);
                    }
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
                    System.Collections.Generic.List<Rental> rentals = dal.GetRentalsOfCustomer(customerId);
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
