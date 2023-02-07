using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _db;
        private readonly IMapper _mapper;
        public DishService(RestaurantDbContext db, IMapper mapper)
        {
        _db = db;
        _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto createDishDto)
        {
            var restaurant = this.GetRestaurantById(restaurantId);

            var DishEntity = _mapper.Map<Dish>(createDishDto);

             DishEntity.RestaurantId= restaurantId;

            _db.Dishes.Add(DishEntity);
            _db.SaveChanges();

            return DishEntity.Id;
        }

        public DishDTO GetById(int restaurantId, int DishId)
        {
            var restaurant = this.GetRestaurantById(restaurantId);

            var dish = _db.Dishes.FirstOrDefault(u=>u.Id == DishId);

            if(dish == null || dish.RestaurantId != restaurantId )
            {
                throw new NotFoundException("Dish not Found");
            }

            var dishDTO = _mapper.Map<DishDTO>(dish);

            return dishDTO;
        }

        public List<DishDTO> Get(int restaurantId)
        {
            var restaurant = this.GetRestaurantById(restaurantId);

            var dishDTOs = _mapper.Map<List<DishDTO>>(restaurant.Dishes);

            return dishDTOs;
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = this.GetRestaurantById(restaurantId);

            _db.Dishes.RemoveRange(restaurant.Dishes);
            _db.SaveChanges();
        }

        public void Remove(int restaurantId,int dishId)
        {
            var restaurant = this.GetRestaurantById(restaurantId);

            var dish = _db.Dishes.FirstOrDefault(u=>u.Id== dishId);

            if(dish == null || dish.RestaurantId != restaurant.id)
            {
                throw new NotFoundException("Dish not found");
            }

            _db.Dishes.Remove(dish);
            _db.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _db.Restaurants.
            Include("Dishes").
            FirstOrDefault(u=>u.id == restaurantId);

            if(restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            return restaurant;
        }

    }
}
