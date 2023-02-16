using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            _accountService.RegisterUser(registerUserDto);

            return Ok();
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDTO dto)
        {
            var token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
