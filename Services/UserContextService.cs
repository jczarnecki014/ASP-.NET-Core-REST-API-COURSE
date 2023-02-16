using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IUserContextService
    {
        IHttpContextAccessor _HttpContextAccessor { get; }
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }

    public class UserContextService : IUserContextService
    {
        public IHttpContextAccessor _HttpContextAccessor { get; }
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
        _HttpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _HttpContextAccessor.HttpContext?.User;
        public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
