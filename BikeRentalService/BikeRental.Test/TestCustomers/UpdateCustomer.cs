using BikeRentalServiceApi;
using BikeRentalServiceApi.Model;
using System.Threading.Tasks;
using Xunit;

namespace BikeRental.Test.TestCustomers
{
    public class UpdateCustomer
    {
        [Fact]
        public async Task UpdateCustomerCorrect()
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
                await dal.AddCustomer(c);
                c.Firstname = "Florian";
                int number = await dal.UpdateCustomer(1, c);
                Assert.True(number >= 0);
            }
        }

        /*[Fact]
        public async Task MissingCustomer()
        {
            using (var dal = new DataAccess())
            {
                dal.InitDatabase();
                Customer c = new Customer();
                c.Birthday = System.DateTime.Now;
                c.Firstname = "Florian";
                c.Lastname = "Mustermann";
                c.Street = "MusterStreet";
                c.Town = "MusterTown";
                c.ZipCode = "1234";
                await Assert.ThrowsAsync<CustomerNotExistingException>(async () => await dal.UpdateCustomer(1,c));
            }
        }*/
    }
}
