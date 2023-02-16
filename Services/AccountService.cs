using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {

        private readonly RestaurantDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(RestaurantDbContext db, IPasswordHasher<User> passwordHasher,AuthenticationSettings authenticationSettings)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public string GenerateJwt(LoginDTO dto)
        {
            var user = _db.Users
            .Include("Role")
            .FirstOrDefault(u=>u.Email == dto.Email);

            if (user == null) 
            {
                throw new BadRequestException("Invalid username or password");
            }

            var resoult = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash,dto.Password);

            if(resoult == PasswordVerificationResult.Failed) 
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
            };

            if(!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add
                (
                    new Claim("Nationality", user.Nationality)
                );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred =  new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken
            (
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires:expires,
                signingCredentials:cred
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

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
