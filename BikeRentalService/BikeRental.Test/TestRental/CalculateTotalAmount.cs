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
                int customerId = await dal.AddCustomer(c);
                Assert.True(customerId >= 0);

                Bike b = new Bike
                {
                    Brand = "Testbrand",
                    PurchaseDate = System.DateTime.Now,
                    RentalPriceFirstHour = 10,
                    RentalPriceAdditionalHours = 20,
                    BikeCategory = BikeCategory.Standard
                };
                int bikeId = await dal.AddBike(b);
                Assert.True(bikeId >= 0);

                Rental rental = await dal.StartRental(customerId, bikeId);

                rental = await dal.StopRental(rental.RentalId, rental.RentalBegin.AddMinutes(15));
                Assert.True(rental.TotalAmount == 0);


            }

        }
        [Fact]
        public async Task TestCalculation45Mintues()
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
                int customerId = await dal.AddCustomer(c);
                Assert.True(customerId >= 0);

                Bike b = new Bike
                {
                    Brand = "Testbrand",
                    PurchaseDate = System.DateTime.Now,
                    RentalPriceFirstHour = 10,
                    RentalPriceAdditionalHours = 20,
                    BikeCategory = BikeCategory.Standard
                };
                int bikeId = await dal.AddBike(b);
                Assert.True(bikeId >= 0);

                Rental rental = await dal.StartRental(customerId, bikeId);

                rental = await dal.StopRental(rental.RentalId, rental.RentalBegin.AddMinutes(45));
                Assert.True(rental.TotalAmount == b.RentalPriceFirstHour);


            }

        }
        [Fact]
        public async Task TestCalculation2HoursAnd30Mintues()
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
                int customerId = await dal.AddCustomer(c);
                Assert.True(customerId >= 0);

                Bike b = new Bike
                {
                    Brand = "Testbrand",
                    PurchaseDate = System.DateTime.Now,
                    RentalPriceFirstHour = 10,
                    RentalPriceAdditionalHours = 20,
                    BikeCategory = BikeCategory.Standard
                };
                int bikeId = await dal.AddBike(b);
                Assert.True(bikeId >= 0);

                Rental rental = await dal.StartRental(customerId, bikeId);

                rental = await dal.StopRental(rental.RentalId, rental.RentalBegin.AddMinutes(150));
                Assert.True(rental.TotalAmount == (b.RentalPriceFirstHour + 2 * b.RentalPriceAdditionalHours));

            }

        }

    }
}
