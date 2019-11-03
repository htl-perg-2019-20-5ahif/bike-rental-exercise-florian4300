using BikeRentalServiceApi;
using BikeRentalServiceApi.Model;
using System.Threading.Tasks;
using Xunit;

namespace BikeRental.Test.TestRental
{
    public class CalculateTotalAmount
    {
        [Fact]
        public async Task TestCalculation15Mintues()
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
                var customerId = await dal.AddCustomer(c);
                Assert.True(customerId >= 0);

                Bike b = new Bike();
                b.Brand = "Testbrand";
                b.PurchaseDate = System.DateTime.Now;
                b.RentalPriceFirstHour = 10;
                b.RentalPriceAdditionalHours = 20;
                b.BikeCategory = BikeCategory.Standard;
                var bikeId = await dal.AddBike(b);
                Assert.True(bikeId >= 0);

                var rental = await dal.StartRental(customerId, bikeId);

                rental = await dal.StopRental(rental.RentalId, rental.RentalBegin.AddMinutes(15));
                Assert.True(rental.TotalAmount == 0);


            }

        }
        [Fact]
        public async Task TestCalculation45Mintues()
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
                var customerId = await dal.AddCustomer(c);
                Assert.True(customerId >= 0);

                Bike b = new Bike();
                b.Brand = "Testbrand";
                b.PurchaseDate = System.DateTime.Now;
                b.RentalPriceFirstHour = 10;
                b.RentalPriceAdditionalHours = 20;
                b.BikeCategory = BikeCategory.Standard;
                var bikeId = await dal.AddBike(b);
                Assert.True(bikeId >= 0);

                var rental = await dal.StartRental(customerId, bikeId);

                rental = await dal.StopRental(rental.RentalId, rental.RentalBegin.AddMinutes(45));
                Assert.True(rental.TotalAmount == b.RentalPriceFirstHour);


            }

        }
        [Fact]
        public async Task TestCalculation2HoursAnd30Mintues()
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
                var customerId = await dal.AddCustomer(c);
                Assert.True(customerId >= 0);

                Bike b = new Bike();
                b.Brand = "Testbrand";
                b.PurchaseDate = System.DateTime.Now;
                b.RentalPriceFirstHour = 10;
                b.RentalPriceAdditionalHours = 20;
                b.BikeCategory = BikeCategory.Standard;
                var bikeId = await dal.AddBike(b);
                Assert.True(bikeId >= 0);

                var rental = await dal.StartRental(customerId, bikeId);

                rental = await dal.StopRental(rental.RentalId, rental.RentalBegin.AddMinutes(150));
                Assert.True(rental.TotalAmount == (b.RentalPriceFirstHour + 2 * b.RentalPriceAdditionalHours));

            }

        }

    }
}
