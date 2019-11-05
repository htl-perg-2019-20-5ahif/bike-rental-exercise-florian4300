using BikeRentalServiceApi.Model;
using Microsoft.AspNetCore.Mvc;
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
        public List<Customer> GetCustomers(string filter)
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
            var customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == customerId);
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
            var c = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            var rentals = GetRentalsOfCustomer(c.CustomerId);
            foreach(var r in rentals)
            {
                var bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == r.BikeId);
                if (bikeFromDb == null)
                {
                    throw new BikeNotExistingException();
                }
                bikeFromDb.RentalId = 0;
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
                availableBikes = context.Bikes.ToList().FindAll(b => b.RentalId > 0).OrderBy(b => b.RentalPriceFirstHour).ToList();
            }
            else if (filter.Equals("priceAdditionalHours"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.RentalId > 0).OrderBy(b => b.RentalPriceAdditionalHours).ToList(); ;
            }
            else if (filter.Equals("purchaseDate"))
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.RentalId > 0).OrderByDescending(b => b.PurchaseDate).ToList(); ;
            }
            else
            {
                availableBikes = context.Bikes.ToList().FindAll(b => b.RentalId <= 0);
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
            var bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
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
            var b = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            if(b.RentalId > 0)
            {
                throw new BikeInRentalException();
            }
            context.Bikes.Remove(b);
            await context.SaveChangesAsync();
            return (b.BikeId);
        }
        public async Task<Rental> StartRental(int customerId, int bikeId)
        {
            var customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == customerId);
            if (customerFromDb == null)
            {
                throw new CustomerNotExistingException();
            }
            var bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == bikeId);
            if (bikeFromDb == null)
            {
                throw new BikeNotExistingException();
            }
            if(bikeFromDb.RentalId > 0)
            {
                throw new BikeAlreadyInRentalException();
            }
             if (GetRentalsOfCustomer(customerFromDb.CustomerId).Count > 0)
             {
               throw new CustomerAlreadyInRentalException();
             } 
            
            Rental rental = new Rental();
            rental.CustomerId = customerId;
            rental.BikeId = bikeId;
            rental.RentalBegin = System.DateTime.Now;
            rental.RentalEnd = null;
            context.Rentals.Add(rental);
            await context.SaveChangesAsync();
            bikeFromDb.RentalId = rental.RentalId;
            context.Bikes.Update(bikeFromDb);
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

            var bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == rental.BikeId);
             if (bikeFromDb != null)
            {
                bikeFromDb.RentalId = 0;
            }
            bikeFromDb.RentalId = 0;
            context.Bikes.Update(bikeFromDb);
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
            var rentals = context.Rentals;
            foreach (var rental in rentals.ToList())
            {
                if (rental.TotalAmount > 0 && rental.RentalEnd != null && rental.Paid == false)
                {
                    var customerFromDb = context.Customers.ToList().Find(c => c.CustomerId == rental.CustomerId);
                    if (customerFromDb == null)
                    {
                        throw new CustomerNotExistingException();
                    }
                    UnpaidRental ur = new UnpaidRental();
                    ur.CustomerId = customerFromDb.CustomerId;
                    ur.Firstname = customerFromDb.Firstname;
                    ur.Lastname = customerFromDb.Lastname;
                    ur.RentalId = rental.RentalId;
                    ur.RentalBegin = rental.RentalBegin;
                    ur.RentalEnd = rental.RentalEnd;
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
            var bikeFromDb = context.Bikes.ToList().Find(b => b.BikeId == rental.BikeId);
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
            foreach(var rental in context.Rentals)
            {
                if(rental.CustomerId == customerId)
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
                this.context.Dispose();
                context = null;
            }
        }
    }
}
