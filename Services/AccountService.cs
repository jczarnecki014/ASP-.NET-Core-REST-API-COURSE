using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {

        private readonly RestaurantDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(RestaurantDbContext db, IPasswordHasher<User> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
        var User = new User()
        {
            Email = registerUserDto.Email,
            DateOfBirth = registerUserDto.DateOfBirth,
            Nationality = registerUserDto.Nationality,
            RoleId = registerUserDto.RoleId
        };

        var password = _passwordHasher.HashPassword(User,registerUserDto.Password);

        User.PasswordHash = password;

        _db.Users.Add(User);
        _db.SaveChanges();
        }

    }
}
