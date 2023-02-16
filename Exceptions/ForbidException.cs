using System;

namespace RestaurantAPI.Exceptions
{
    public class ForbidException:Exception
    {
        public ForbidException(string errorMessage):base(errorMessage)
        {
        }
    }
}
