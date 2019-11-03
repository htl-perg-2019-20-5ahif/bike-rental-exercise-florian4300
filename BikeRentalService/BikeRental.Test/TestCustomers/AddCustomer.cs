using BikeRentalServiceApi;
using BikeRentalServiceApi.Model;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BikeRental.Test.TestCustomers
{

    public class AddCustomer
    {
        [Fact]
        public async Task AddCustomerCorrect()
        {
            using (var dal = new DataAccess())
            {

                dal.InitDatabase();
                Customer c = new Customer();
                c.Birthday = System.DateTime.Now;
                c.Firstname = "Max";
                c.Lastname = "Mustermann";
                c.Street = "MusterStreet";
                c.Town = "MusterTown";
                c.ZipCode = "1234";
                var number = await dal.AddCustomer(c);
                Assert.True(number >= 0);
            }
        }

        [Fact]
        public async Task MissingLastname()
        {
            using (var dal = new DataAccess())
            {
                dal.InitDatabase();
                Customer c = new Customer();
                c.Birthday = System.DateTime.Now;
                c.Firstname = "Max";
                c.Street = "MusterStreet";
                c.Town = "MusterTown";
                c.ZipCode = "1234";
                await Assert.ThrowsAsync<ArgumentException>(async () => await dal.AddCustomer(c));
            }
        }
    }
}
