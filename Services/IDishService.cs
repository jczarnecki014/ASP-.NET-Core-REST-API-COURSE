using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto createDishDto);
        List<DishDTO> Get(int restaurantId);
        DishDTO GetById(int restaurantId, int DishId);
        public void RemoveAll(int restaurantId);
        public void Remove(int restaurantId,int dishId);
    }
}