using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumAmountOfRestaurantsRequirement:IAuthorizationRequirement
    {
        public int minimumAmountOfRestaurants { get;}
        public MinimumAmountOfRestaurantsRequirement(int minimumAmountOfRestaurants)
        {
            this.minimumAmountOfRestaurants = minimumAmountOfRestaurants;
        }
    }
}
