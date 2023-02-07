﻿using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class UpdateRestaurantDTO
    {
         [Required]
         [MaxLength(25)]
         public string name { get;set; }
         public string description { get;set; }
         public bool hasDelivery { get;set; }
    }
}