using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Entities
{
    public class Restaurant
    {
        public int id { get;set; }
        public string name { get;set; }
        public string description { get;set; }
        public string Category { get;set; }
        public bool hasDelivery { get;set; }
        public string ContactEmail { get;set; }
        public string ContactNumber{ get;set; }
        public int? CreatedById { get;set; }
        public virtual User CreatedBy { get;set; }
        public int AddressId{ get;set; }
        public virtual Address address { get;set; }
        public virtual List<Dish> Dishes { get;set; }
    }
}
