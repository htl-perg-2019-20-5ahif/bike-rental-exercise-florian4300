using BikeRentalServiceApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BikeRentalServiceApi.Exceptions;

namespace BikeRentalServiceApi
{
    public class DataAccess : IDataAccess
    {
        public BikeRentalContext context;


        public void InitDatabase()
        {
            this.context = new BikeRentalContext();
        }
        public List<Customer> GetCustomers( string filter)
        {
            if (filter != null)
            {
                var filteredCustomers = context.Customers.ToList().FindAll(c => c.Lastname.ToLower().Contains(filter.ToLower()));
                if (filteredCustomers.Any())
                {
                    return (filteredCustomers);
                }
                else
                {
                    return null;
                }
            }
            return (context.Customers.ToList());
        }

        public async Task<int> AddCustomer( Customer customer)
        {
            if(customer.Lastname == null || customer.Firstname == null ||customer.Birthday == null || customer.Street == null || customer.Town == null || customer.ZipCode == null)
            {
                throw new ArgumentException();
            }
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return (customer.CustomerId);
        }


        public async Task<int> UpdateCustomer(int customerId, Customer customer)
        {
            var customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            if (customerFromDb == null)
            {
                throw new CustomerNotExistingException ();
            }
            customerFromDb.Birthday = customer.Birthday;
            customerFromDb.Firstname = customer.Firstname;
            customerFromDb.Lastname = customer.Lastname;
            customerFromDb.HouseNumber = customer.HouseNumber;
            customerFromDb.Street = customer.Street;
            customerFromDb.Town = customer.Town;
            customerFromDb.ZipCode = customer.ZipCode;
            context.Customers.Update(customerFromDb);
            await context.SaveChangesAsync();
            return (customerFromDb.CustomerId);
        }

        public async Task<Customer> DeleteCustomer(int customerId)
        {
            if (context.Customers.ToList().Find(c => c.CustomerId == customerId) == null)
            {
                return null;
            }
            var c = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            context.Customers.Remove(c);
            await context.SaveChangesAsync();
            return (c);
        }

        public List<Rental> GetRentalsOfCustomer(int customerId)
        {
            if (context.Customers.ToList().Find(c => c.CustomerId == customerId) == null)
            {
                return null;
            }
            var c = context.Customers.ToList().Find(c => c.CustomerId == customerId);

            return (null);
        }
        public List<Bike> GetBikes(string filter)
        {
            List<Bike> availableBikes;
            if (filter.Equals("priceFirstHour"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null).OrderBy(b => b.RentalPriceFirstHour).ToList();
            }
            else if (filter.Equals("priceAdditionalHours"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null).OrderBy(b => b.RentalPriceAdditionalHours).ToList(); ;
            }
            else if (filter.Equals("purchaseData"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null).OrderBy(b => b.RentalPriceAdditionalHours).ToList(); ;
            }
            else
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.Rental == null);
            }
            return (availableBikes);
        }
        public async Task<int> AddBike( Bike bike)

        {
            await context.Bikes.AddAsync(bike);
            await context.SaveChangesAsync();
            return (bike.BikeId);
        }


        public async Task<int> UpdateBike(int bikeId,  Bike bike)
        {
            var bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            if (bikeFromDb == null)
            {
                throw new BikeNotExistingException();
            }

            bikeFromDb.Brand = bike.Brand;
            bikeFromDb.LastServiceDate = bike.LastServiceDate;
            bikeFromDb.Notes = bike.Notes;
            bikeFromDb.PurchaseDate = bike.PurchaseDate;
            bikeFromDb.RentalPriceAdditionalHours = bike.RentalPriceAdditionalHours;
            bikeFromDb.RentalPriceFirstHour = bike.RentalPriceFirstHour;
            context.Update(bikeFromDb);
            await context.SaveChangesAsync();
            return (bikeFromDb.BikeId);

        }

        public async Task<Bike> DeleteBike(int bikeId)
        {
            if (context.Bikes.ToList().Find(b => b.BikeId == bikeId) == null)
            {
                throw new BikeNotExistingException();
            }
            var b = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            context.Bikes.Remove(b);
            await context.SaveChangesAsync();
            return (b);
        }
        // GET: api/Rentals
        public IEnumerable<Rental> GetRentals()
        {
            return context.Rentals.ToList();
        }


        public async Task<Rental> GetRental(int id)
        {
            var rental = await context.Rentals.FindAsync(id);

            if (rental == null)
            {
                throw new RentalNotExistingException();
            }

            return rental;
        }

        // PUT: api/Rentals/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.


        // POST: api/Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.

        public async Task<Rental> StartRental(int customerId, int bikeId)
        {
            Rental rental = new Rental();
            rental.CustomerId = customerId;
            rental.BikeId = bikeId;
            rental.RentalBegin = System.DateTime.Now;
            rental.RentalEnd = null;
            context.Rentals.Add(rental);
            await context.SaveChangesAsync();

            return rental;
        }

        public async Task<Rental> StopRental(int rentalId, DateTime End)
        {
            var rental = this.GetRentalById(rentalId);
            if (rental == null)
            {
                throw new RentalNotExistingException();
            }
            if(rental.RentalEnd != null)
            {
                throw new RentalAlreadyEndedException();
            }
            rental.RentalEnd = End;
            if(rental.RentalBegin.CompareTo(rental.RentalEnd) > 0)
            {
                throw new ArgumentException();
            }
            rental.TotalAmount = CalculateTotalPrice(rental);
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();

            return rental;
        }


        public async Task<Rental> payRental(int rentalId)
        {
            var rental = this.GetRentalById(rentalId);
            if (rental == null)
            {
                throw new RentalNotExistingException();
            }
            if (rental.RentalEnd == null)
            {
                throw new RentalNotEndedException();
            }
            if (rental.TotalAmount > 0)
            {
                throw new TotalAmountNotGreaterThanZeroException();
            }
            rental.Paid = true;
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();

            return rental;
        }

        public List<UnpaidRental> GetUnpaid(int id)
        {
            List<UnpaidRental> unpaidRentals = new List<UnpaidRental>();
            var rentals = context.Rentals;
            foreach (var rental in rentals)
            {
                if (rental.TotalAmount > 0 && rental.RentalEnd != null && rental.Paid == false)
                {
                    UnpaidRental ur = new UnpaidRental();
                    ur.CustomerId = rental.Customer.CustomerId;
                    ur.Firstname = rental.Customer.Firstname;
                    ur.Lastname = rental.Customer.Lastname;
                    ur.RentalId = rental.RentalId;
                    ur.RentalBegin = ur.RentalBegin;
                    ur.RentalEnd = ur.RentalEnd;
                }
            }
            return unpaidRentals;


        }



        public Rental GetRentalById(int id)
        {
            return context.Rentals.Where(e => e.RentalId == id).FirstOrDefault();
        }
        public double CalculateTotalPrice(Rental rental)
        {
            double totalCost = 0.0;
            if (rental.RentalBegin != null && rental.RentalEnd != null)
            {
                TimeSpan ts = (TimeSpan)(rental.RentalEnd - rental.RentalBegin);
                if (ts.TotalMinutes <= 15)
                {
                    return 0;
                }
                ts = ts.Subtract(TimeSpan.FromHours(1));
                totalCost += rental.Bike.RentalPriceFirstHour;
                while (ts.TotalMinutes > 0)
                {
                    ts = ts.Subtract(TimeSpan.FromHours(1));
                    totalCost += rental.Bike.RentalPriceAdditionalHours;
                }

            }

            return totalCost;

        }

        public void Dispose()
        {
            if (context != null)
            {
                this.context.Dispose();
            }
        }
    }
}
