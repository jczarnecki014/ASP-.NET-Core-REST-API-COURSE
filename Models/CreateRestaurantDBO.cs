using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class CreateRestaurantDBO
    {
        public int id { get;set; }
        [Required]
        [MaxLength(25)]
        public string name { get;set; }
        public string description { get;set; }
        public string Category { get;set; }
        public bool hasDelivery { get;set; }
        public string ContactEmail { get;set; }
        public string ContactNumber{ get;set; }
        [MaxLength(50)]
        public string City { get;set; }
        [MaxLength(50)]
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}
