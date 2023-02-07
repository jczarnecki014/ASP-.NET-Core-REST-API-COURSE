using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("/api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpPost]
        public IActionResult Create([FromRoute] int restaurantId, [FromBody] CreateDishDto createDishDto )
        {
            var Id = _dishService.Create(restaurantId,createDishDto);

            return Created($"/api/restaurant/{restaurantId}/dish/{Id}",null);
        }

        [HttpGet]
        [Route("{dishId}")]
        public ActionResult GetById([FromRoute] int restaurantId, [FromRoute] int dishId )
        {

            var dishDto = _dishService.GetById(restaurantId,dishId);
            return Ok(dishDto);
            
        }

        [HttpGet]
        public ActionResult Get([FromRoute] int restaurantId )
        {
            var dishDto = _dishService.Get(restaurantId);
            return Ok(dishDto);
        }

        [HttpDelete]
        public ActionResult RemoveAll([FromRoute] int restaurantId )
        {
            _dishService.RemoveAll(restaurantId);

            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public ActionResult Remove([FromRoute] int restaurantId, [FromRoute] int dishId )
        {
            _dishService.Remove(restaurantId,dishId);

            return NoContent();
        }

    }
}
