using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BikeRentalServiceApi.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {

        private readonly IDataAccess dal;
        private readonly BikeRentalContext context = new BikeRentalContext();

        public CustomerController(IDataAccess _dal)
        {
            dal = _dal;
        }

        [HttpGet]
        public ActionResult GetCustomers([FromQuery] string filter)
        {
            using (dal)
            {
                var customers = dal.GetCustomers(filter);
                return Ok(customers);

            }
            //if (filter != null)
            //{
            //    var filteredCustomers = context.Customers.ToList().FindAll(c => c.Lastname.ToLower().Contains(filter.ToLower()));
            //    if (filteredCustomers.Any())
            //    {
            //        return Ok(filteredCustomers);
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }
            //}
            //return Ok(context.Customers);
        }
        [HttpPost]
        public ActionResult AddCustomer([FromBody] Customer customer)
        {
            using (dal)
            {
                try
                {
                    var id = dal.AddCustomer(customer);
                    return Ok(id);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }

            }

            //context.Customers.Add(customer);
            //context.SaveChanges();
            //return Ok(customer);
        }
        [HttpPut]
        [Route("{customerId}")]
        public ActionResult UpdateCustomer(int customerId, [FromBody] Customer customer)
        {
            using (dal)
            {
                var updatedCustomer = dal.UpdateCustomer(customerId, customer);
                if (updatedCustomer == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(customer);
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
        public ActionResult DeleteCustomer(int customerId)
        {
            using (dal)
            {
                var deletedCustomer = dal.DeleteCustomer(customerId);
                if (deletedCustomer == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(deletedCustomer);
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
        [Route("{customerId}")]
        public ActionResult GetRentalsOfCustomer(int customerId)
        {
            using (dal)
            {
                var rentals = dal.GetRentalsOfCustomer(customerId);
                if (rentals == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(rentals);
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
