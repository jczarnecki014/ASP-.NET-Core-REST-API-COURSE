using AutoMapper;
using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
    public interface IRestaurantServices
    {
        public int Create(CreateRestaurantDBO dto);
        public void Delete(int id);
        public List<RestaurantDTO> GetAll();
        public RestaurantDTO GetById(int id);
        public void Update(int id, UpdateRestaurantDTO dto);
    }
}