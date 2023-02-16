using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private IRestaurantServices _Restaurant_Services { get; }

        public RestaurantController(IRestaurantServices _restaurant_services)
        {
            _Restaurant_Services = _restaurant_services;
        }
        [HttpGet]
        /*[Authorize(Policy = "AtLeast20")]*/
        /*[Authorize(Policy = "MinNumberOfRestaurant")]*/
        public ActionResult GetAll([FromQuery] RestaurantQuery query)
        {
            var restaurants = _Restaurant_Services.GetAll(query);
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
        [Authorize(Roles = "Admin,Manager")]
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
