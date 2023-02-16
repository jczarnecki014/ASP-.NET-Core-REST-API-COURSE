using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        string GenerateJwt(LoginDTO dto);
        void RegisterUser(RegisterUserDto registerUserDto);
    }
}