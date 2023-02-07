using RestaurantAPI.Entities;
using System.Collections.Generic;

namespace RestaurantAPI.Models
{
    public class RestaurantDTO
    {
        public int Id { get; set; }
        public string name { get;set; }
        public string description { get;set; }
        public string Category { get;set; }
        public bool hasDelivery { get;set; }
        public string City { get;set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public List<DishDTO> Dishes{ get; set; }
    }
}
