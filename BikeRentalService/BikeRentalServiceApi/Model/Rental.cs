﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalServiceApi.Model
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        [Required]
        [ForeignKey("Customer")]

        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("Bike")]
        public int BikeId { get; set; }


        public DateTime RentalBegin { get; set; }

        public DateTime? RentalEnd { get; set; }

        [Range(0.0, double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public double TotalAmount { get; set; }


        public bool Paid { get; set; }
    }
    [NotMapped]
    public class UnpaidRental
    {
        public int RentalId { get; set; }
        public DateTime RentalBegin { get; set; }

        public DateTime? RentalEnd { get; set; }

        public int CustomerId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
    [NotMapped]
    public class CustomerAndBikeId
    {
        public int CustomerId { get; set; }
        public int BikeId { get; set; }
    }

    [NotMapped]
    public class RentalApi
    {
        [Key]
        public int RentalId { get; set; }

        [Required]
        [ForeignKey("Customers")]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("Bikes")]
        public int BikeId { get; set; }


        public DateTime RentalBegin { get; set; }

        public DateTime? RentalEnd { get; set; }

        [Range(0.0, double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public double TotalAmount { get; set; }


        public bool Paid { get; set; }

        public static explicit operator RentalApi(Rental r)
        {
            RentalApi rapi = new RentalApi
            {
                RentalId = r.RentalId,
                BikeId = r.BikeId,
                CustomerId = r.CustomerId,
                Paid = r.Paid,
                RentalBegin = r.RentalBegin,
                RentalEnd = r.RentalEnd,
                TotalAmount = r.TotalAmount
            };
            return rapi;
        }
    }
}
