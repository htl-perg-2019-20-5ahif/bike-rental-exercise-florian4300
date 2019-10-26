using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BikeRentalServiceApi.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private static List<Customer> customers = new List<Customer>();

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetCustomers([FromQuery] string filter)
        {
            if(filter != null)
            {
                var filteredCustomers = customers.FindAll(c => c.Lastname.Contains(filter));
                if (filteredCustomers.Any())
                {
                    return Ok(filteredCustomers);
                } else
                {
                    return NotFound();
                }
            }
            return Ok(customers);
        }
        [HttpPost]
        public ActionResult AddCustomer([FromBody] Customer customer)
        {
            return Ok(customer);
        }
        [HttpPut]
        [Route("{customerId}")]
        public ActionResult UpdateCustomer(int customerId)
        {
            if(customerId >= customers.Count)
            {
                return BadRequest();
            }
            var c = customers.Find(c => c.CustomerId == customerId);
            return Ok(c);
        }
        [HttpDelete]
        [Route("{customerId}")]
        public ActionResult DeleteCustomer(int customerId)
        {
            if (customerId >= customers.Count)
            {
                return BadRequest();
            }
            var c = customers.Find(c => c.CustomerId == customerId);
            customers.Remove(c);
            return Ok(c);
        }

        [HttpGet]
        [Route("{customerId}")]
        public ActionResult GetRentals(int customerId)
        {
            if (customerId >= customers.Count)
            {
                return BadRequest();
            }
            var c = customers.Find(c => c.CustomerId == customerId);
            
            return Ok(c);
        }

    }
}
