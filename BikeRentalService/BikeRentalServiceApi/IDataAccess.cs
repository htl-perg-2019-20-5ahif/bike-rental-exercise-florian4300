﻿using BikeRentalServiceApi.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeRentalServiceApi
{
    public interface IDataAccess : IDisposable
    {
        public void InitDatabase();
        public List<Customer> GetCustomers(string filter);

        public Task<int> AddCustomer(Customer customer);


        public Task<int> UpdateCustomer(int customerId, Customer customer);

        public Task<int> DeleteCustomer(int customerId);
        public List<Rental> GetRentalsOfCustomer(int customerId);
        public List<Bike> GetBikes(string filter);
        public Task<int> AddBike(Bike bike);

        public Task<int> UpdateBike(int bikeId, Bike bike);

        public Task<int> DeleteBike(int bikeId);
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.


        // POST: api/Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.

        public Task<Rental> StartRental(int customerId, int bikeId);
        public Task<Rental> StopRental(int rentalId, DateTime end);


        public Task<Rental> payRental(int rentalId);

        public List<UnpaidRental> GetUnpaid();



        public Rental GetRentalById(int id);

        public double CalculateTotalPrice(Rental rental);

    }
}
