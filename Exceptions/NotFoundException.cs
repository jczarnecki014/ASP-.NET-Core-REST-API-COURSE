using System;

namespace RestaurantAPI.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string errorMessage):base(errorMessage)
        {
        }
    }
}
