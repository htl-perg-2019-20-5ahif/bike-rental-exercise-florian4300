using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalServiceApi.Model
{
    public enum Gender { Male, Female, Unknown }
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(75)]
        public string Lastname { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(75)]
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

    public class ApiCustomer
    {

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(75)]
        public string Lastname { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(75)]
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
