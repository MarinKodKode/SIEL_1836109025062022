namespace SIEL_1836109025062022.Services
{

    public interface IUserService
    {
        int GetUserId();
    }
    public class UserService : IUserService
    {
        public int GetUserId()
        {
           return 1;
        }

    }
}
