using FluentValidation;
using RestaurantAPI.Entities;
using System.Linq;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserDTOValidator: AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDTOValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x=> x.Email)
            .NotEmpty()
            .EmailAddress()
            .Custom((value,context) => 
            {
                var emailInUse = dbContext.Users.Any(u=>u.Email== value);
                if(emailInUse) 
                {
                    context.AddFailure("Email","That email is taken");
                }
            });

            RuleFor(x=> x.Password)
            .NotEmpty();

            RuleFor(x=>x.ConfirmPassword)
            .Equal(x=>x.Password);


        }
    }
}
