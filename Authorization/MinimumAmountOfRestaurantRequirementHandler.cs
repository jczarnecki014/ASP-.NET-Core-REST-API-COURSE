using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class MinimumAmountOfRestaurantRequirementHandler : AuthorizationHandler<MinimumAmountOfRestaurantsRequirement>
    {
        private readonly RestaurantDbContext _db;

        public MinimumAmountOfRestaurantRequirementHandler(RestaurantDbContext db)
        {
        _db = db;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAmountOfRestaurantsRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c=>c.Type==ClaimTypes.NameIdentifier).Value);

            int restaurantCount = _db.Restaurants.Where(u=>u.CreatedById == userId).Count();

            if(restaurantCount >= requirement.minimumAmountOfRestaurants)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
