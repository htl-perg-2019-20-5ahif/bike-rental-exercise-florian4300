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
            context = new BikeRentalContext();

        }
        public List<Customer> GetCustomers(string filter)
        {
            if (filter != null)
            {
                List<Customer> filteredCustomers = context.Customers.ToList().FindAll(c => c.Lastname.ToLower().Contains(filter.ToLower()));
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

        public async Task<int> AddCustomer(Customer customer)
        {
            if (customer.Lastname == null || customer.Firstname == null || customer.Birthday == null || customer.Street == null || customer.Town == null || customer.ZipCode == null)
            {
                throw new ArgumentException();
            }
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return (customer.CustomerId);
        }


        public async Task<int> UpdateCustomer(int customerId, Customer customer)
        {
            Customer customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            if (customerFromDb == null)
            {
                throw new CustomerNotExistingException();
            }
            customerFromDb.Gender = customer.Gender;
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

        public async Task<int> DeleteCustomer(int customerId)
        {
            if (context.Customers.ToList().Find(c => c.CustomerId == customerId) == null)
            {
                throw new CustomerNotExistingException();
            }
            Customer c = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            List<Rental> rentals = GetRentalsOfCustomer(c.CustomerId);
            foreach (Rental r in rentals)
            {
                Bike bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == r.BikeId);
                if (bikeFromDb == null)
                {
                    throw new BikeNotExistingException();
                }
                bikeFromDb.ActiveRentalId = 0;
            }
            await context.SaveChangesAsync();
            context.Rentals.RemoveRange(rentals);
            await context.SaveChangesAsync();

            context.Customers.Remove(c);
            await context.SaveChangesAsync();
            return (c.CustomerId);
        }

        public List<Bike> GetBikes(string filter)
        {
            List<Bike> availableBikes;
            if (filter.Equals("priceFirstHour"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.ActiveRentalId > 0).OrderBy(b => b.RentalPriceFirstHour).ToList();
            }
            else if (filter.Equals("priceAdditionalHours"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.ActiveRentalId > 0).OrderBy(b => b.RentalPriceAdditionalHours).ToList(); ;
            }
            else if (filter.Equals("purchaseDate"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.ActiveRentalId > 0).OrderByDescending(b => b.PurchaseDate).ToList(); ;
            }
            else
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.ActiveRentalId <= 0);
            }
            return (availableBikes);
        }
        public async Task<int> AddBike(Bike bike)

        {
            await context.Bikes.AddAsync(bike);
            await context.SaveChangesAsync();
            return (bike.BikeId);
        }


        public async Task<int> UpdateBike(int bikeId, Bike bike)
        {
            Bike bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            if (bikeFromDb == null)
            {
                throw new BikeNotExistingException();
            }
            bikeFromDb.BikeCategory = bike.BikeCategory;
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

        public async Task<int> DeleteBike(int bikeId)
        {
            if (context.Bikes.ToList().Find(b => b.BikeId == bikeId) == null)
            {
                throw new BikeNotExistingException();
            }
            Bike b = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            if (b.ActiveRentalId > 0)
            {
                throw new BikeInRentalException();
            }
            context.Bikes.Remove(b);
            await context.SaveChangesAsync();
            return (b.BikeId);
        }
        public async Task<Rental> StartRental(int customerId, int bikeId)
        {
            Customer customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            if (customerFromDb == null)
            {
                throw new CustomerNotExistingException();
            }
            Bike bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            if (bikeFromDb == null)
            {
                throw new BikeNotExistingException();
            }
            if (bikeFromDb.ActiveRentalId > 0)
            {
                throw new BikeAlreadyInRentalException();
            }
            if (GetRentalsOfCustomer(customerFromDb.CustomerId).Count > 0)
            {
                throw new CustomerAlreadyInRentalException();
            }

            Rental rental = new Rental
            {
                CustomerId = customerId,
                BikeId = bikeId,
                RentalBegin = System.DateTime.Now,
                RentalEnd = null
            };
            context.Rentals.Add(rental);
            await context.SaveChangesAsync();
            bikeFromDb.ActiveRentalId = rental.RentalId;
            context.Bikes.Update(bikeFromDb);
            await context.SaveChangesAsync();


            return rental;
        }

        public async Task<Rental> StopRental(int rentalId, DateTime End)
        {
            Rental rental = GetRentalById(rentalId);
            if (rental == null)
            {
                throw new RentalNotExistingException();
            }
            if (rental.RentalEnd != null)
            {
                throw new RentalAlreadyEndedException();
            }
            rental.RentalEnd = End;
            if (rental.RentalBegin.CompareTo(rental.RentalEnd) > 0)
            {
                throw new ArgumentException();
            }
            rental.TotalAmount = CalculateTotalPrice(rental);

            Bike bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == rental.BikeId);
            if (bikeFromDb != null)
            {
                bikeFromDb.ActiveRentalId = 0;
            }
            bikeFromDb.ActiveRentalId = 0;
            context.Bikes.Update(bikeFromDb);
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();

            return rental;
        }


        public async Task<Rental> payRental(int rentalId)
        {
            Rental rental = GetRentalById(rentalId);
            if (rental == null)
            {
                throw new RentalNotExistingException();
            }
            if (rental.RentalEnd == null)
            {
                throw new RentalNotEndedException();
            }
            if (rental.TotalAmount <= 0)
            {
                throw new TotalAmountNotGreaterThanZeroException();
            }
            rental.Paid = true;
            context.Rentals.Update(rental);
            await context.SaveChangesAsync();

            return rental;
        }

        public List<UnpaidRental> GetUnpaid()
        {
            List<UnpaidRental> unpaidRentals = new List<UnpaidRental>();
            Microsoft.EntityFrameworkCore.DbSet<Rental> rentals = context.Rentals;
            foreach (Rental rental in rentals.ToList())
            {
                if (rental.TotalAmount > 0 && rental.RentalEnd != null && rental.Paid == false)
                {
                    Customer customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == rental.CustomerId);
                    if (customerFromDb == null)
                    {
                        throw new CustomerNotExistingException();
                    }
                    UnpaidRental ur = new UnpaidRental
                    {
                        CustomerId = customerFromDb.CustomerId,
                        Firstname = customerFromDb.Firstname,
                        Lastname = customerFromDb.Lastname,
                        RentalId = rental.RentalId,
                        RentalBegin = rental.RentalBegin,
                        RentalEnd = rental.RentalEnd
                    };
                    unpaidRentals.Add(ur);
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
            Bike bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == rental.BikeId);
            if (bikeFromDb == null)
            {
                throw new BikeNotExistingException();
            }
            double totalCost = 0.0;
            if (rental.RentalBegin != null && rental.RentalEnd != null)
            {
                TimeSpan ts = (TimeSpan)(rental.RentalEnd - rental.RentalBegin);
                if (ts.TotalMinutes <= 15)
                {
                    return 0;
                }
                ts = ts.Subtract(TimeSpan.FromHours(1));
                totalCost += bikeFromDb.RentalPriceFirstHour;
                while (ts.TotalMinutes > 0)
                {
                    ts = ts.Subtract(TimeSpan.FromHours(1));
                    totalCost += bikeFromDb.RentalPriceAdditionalHours;
                }

            }

            return totalCost;

        }
        public List<Rental> GetRentalsOfCustomer(int customerId)
        {
            if (context.Customers.ToList().Find(c => c.CustomerId == customerId) == null)
            {
                throw new CustomerNotExistingException();
            }
            List<Rental> rentalsOfCustomer = new List<Rental>();
            foreach (Rental rental in context.Rentals)
            {
                if (rental.CustomerId == customerId)
                {
                    rentalsOfCustomer.Add(rental);
                }
            }
            return rentalsOfCustomer;
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }
    }
}
