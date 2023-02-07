using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private IRestaurantServices _Restaurant_Services { get; }

        public RestaurantController(IRestaurantServices _restaurant_services)
        {
            _Restaurant_Services = _restaurant_services;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var restaurants = _Restaurant_Services.GetAll();
            return Ok(restaurants);
        }
        [HttpGet("{id}")]
        public ActionResult<RestaurantDTO> Get(int id) 
        {
            
            var restaurant = _Restaurant_Services.GetById(id);

            if(restaurant == null) return NotFound();

            return Ok(restaurant);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDBO restaurant)
        {
            var id = _Restaurant_Services.Create(restaurant);
            return Created($"api/restaurants/{id}",null);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteRestaurant([FromRoute]int id)
        {
           _Restaurant_Services.Delete(id);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody]UpdateRestaurantDTO dto )
        {

            _Restaurant_Services.Update(id, dto);
          
            return Ok();
        }

    }
}
