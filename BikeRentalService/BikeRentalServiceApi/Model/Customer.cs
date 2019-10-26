﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalServiceApi.Model
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public enum Sex { Male, Female, Unknown }

        [Required][MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(75)]
        public string Lastname { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
        [Required][MaxLength(75)]
        public string Street { get; set; }

        [MaxLength(10)]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(75)]
        public string Town { get; set; }
    }
}