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
            using (DataAccess dal = new DataAccess())
            {

                dal.InitDatabase();
                Customer c = new Customer
                {
                    Birthday = System.DateTime.Now,
                    Firstname = "Max",
                    Lastname = "Mustermann",
                    Street = "MusterStreet",
                    Town = "MusterTown",
                    ZipCode = "1234"
                };
                int number = await dal.AddCustomer(c);
                Assert.True(number >= 0);
            }
        }

        [Fact]
        public async Task MissingLastname()
        {
            using (DataAccess dal = new DataAccess())
            {
                dal.InitDatabase();
                Customer c = new Customer
                {
                    Birthday = System.DateTime.Now,
                    Firstname = "Max",
                    Street = "MusterStreet",
                    Town = "MusterTown",
                    ZipCode = "1234"
                };
                await Assert.ThrowsAsync<ArgumentException>(async () => await dal.AddCustomer(c));
            }
        }
    }
}
