using System.Security.Claims;

namespace SIEL_1836109025062022.Services
{

    public interface IUserService
    {
        int GetUserId();
    }
    public class UserService : IUserService
    {
        private readonly HttpContext httpContext;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public int GetUserId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var id = httpContext.User.Claims.
                    Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id_user = int.Parse(id.Value);  
                return id_user;

            }
            else
            {
                return 0;
            }
        }

    }
}
