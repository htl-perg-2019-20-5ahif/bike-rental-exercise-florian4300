using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalServiceApi.Model
{
    public class Bike
    {
        [Key]
        public int BikeId { get; set; }

        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [MaxLength(25)]
        public string Notes { get; set; }

        public DateTime LastServiceDate { get; set; }

        [Required]
        [Range(0.0,Double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]

        public double RentalPriceFirstHour { get; set; }

        [Required]
        [Range(1.0, Double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public double RentalPriceAdditionalHours { get; set; }

        public enum BikeCategory { Standard, Mountain, Trecking, Racing }
    }
}
