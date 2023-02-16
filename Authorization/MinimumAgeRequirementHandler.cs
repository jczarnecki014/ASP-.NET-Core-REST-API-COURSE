using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        public ILogger<MinimumAgeRequirementHandler> _Logger { get; }

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _Logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c=>c.Type == "DateOfBirth").Value);

            var userEmail = context.User.FindFirst(ClaimTypes.Name).Value;

            _Logger.LogInformation($"User: {userEmail} with date of birth [{dateOfBirth}]");

            if(dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Now) 
            {
                _Logger.LogInformation("Authorization succedded");
                context.Succeed(requirement);
            }
            else
            {
                _Logger.LogInformation("Authorization failed");
            }

            return Task.CompletedTask ;
        }
    }
}
