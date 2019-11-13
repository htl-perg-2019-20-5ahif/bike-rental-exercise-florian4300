using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalServiceApi.Model
{
    public enum BikeCategory { Standard, Mountain, Trecking, Racing }
    public class Bike
    {
        [Key]
        public int BikeId { get; set; }

        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public DateTime LastServiceDate { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]

        public double RentalPriceFirstHour { get; set; }

        [Required]
        [Range(1.0, double.MaxValue)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public double RentalPriceAdditionalHours { get; set; }

        [Required]
        public BikeCategory BikeCategory { get; set; }
        public int ActiveRentalId { get; set; }
    }
}
