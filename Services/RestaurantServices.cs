using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Services
{
    public class RestaurantServices : IRestaurantServices
    {
        private readonly RestaurantDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RestaurantServices(RestaurantDbContext db, IMapper mapper, ILogger<RestaurantServices> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        public List<RestaurantDTO> GetAll()
        {
            var restaurants = _db.Restaurants.Include("address").Include("Dishes");

            var restaurantsDTO = _mapper.Map<List<RestaurantDTO>>(restaurants);

            return restaurantsDTO;
        }

        public RestaurantDTO GetById(int id)
        {
            var restaurant = _db.Restaurants.Include("address").Include("Dishes").FirstOrDefault(u => u.id == id);

            if (restaurant == null) return null;

            var restaurantDTO = _mapper.Map<RestaurantDTO>(restaurant);

            return restaurantDTO;

        }

        public int Create(CreateRestaurantDBO dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _db.Restaurants.Add(restaurant);
            _db.SaveChanges();
            return restaurant.id;

        }
        public void Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invkoed");
            var restaurant = _db.Restaurants.FirstOrDefault(u=>u.id==id);
            if(restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            _db.Restaurants.Remove(restaurant);
            _db.SaveChanges();

        }

        public void Update(int id,UpdateRestaurantDTO dto)
        {
            var restaurant = _db.Restaurants.FirstOrDefault(u=>u.id==id);
            
            if(restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            restaurant.name = dto.name;
            restaurant.description = dto.description;
            restaurant.hasDelivery = dto.hasDelivery;

            _db.SaveChanges();

        }


        public IMapper Mapper { get; }
        public ILogger<RestaurantServices> Logger { get; }
    }
}
