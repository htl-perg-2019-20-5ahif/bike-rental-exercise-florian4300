using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalServiceApi.Model
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        [Required]
        [ForeignKey("Customer")]

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        [ForeignKey("Bike")]
        public int BikeId { get; set; }
        public Bike Bike { get; set; }


        public DateTime RentalBegin { get; set; }

        public DateTime? RentalEnd { get; set; }

        [Range(0.0, Double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public double TotalAmount { get; set; }


        public bool Paid { get; set; }
    }
    [NotMapped]
    public class UnpaidRental
    {
        public int RentalId { get; set; }
        public DateTime RentalBegin { get; set; }

        public DateTime RentalEnd { get; set; }

        public int CustomerId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
