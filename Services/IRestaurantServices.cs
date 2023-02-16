using AutoMapper;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IRestaurantServices
    {
        public int Create(CreateRestaurantDBO dto);
        public void Delete(int id);
        public PageResoult<RestaurantDTO> GetAll(RestaurantQuery query);
        public RestaurantDTO GetById(int id);
        public void Update(int id, UpdateRestaurantDTO dto);
    }
}