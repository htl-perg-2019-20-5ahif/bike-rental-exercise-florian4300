using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalServiceApi.Model
{
    public class Rental
    {
        [Required]
        public Customer customer { get; set; }

        [Required]
        public Bike bike { get; set; }

        [Required]
        public DateTime RentalBegin { get; set; }

        public DateTime RentalEnd { get; set; }

        [Range(0.0, Double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public double totalAmount { get; set; }


        public bool Paid { get; set; }
    }
}
