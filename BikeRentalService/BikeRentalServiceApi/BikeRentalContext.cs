using BikeRentalServiceApi.Model;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalServiceApi
{
    public class BikeRentalContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Bike> Bikes { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BikeRental;Trusted_Connection=True");
        }
    }
}
