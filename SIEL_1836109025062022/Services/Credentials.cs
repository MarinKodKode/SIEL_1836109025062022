using SIEL_1836109025062022.Models.Credentials;

namespace SIEL_1836109025062022.Services
{
    public interface ICredentialsRepository
    {
        Task<Credential> GetCredentials(int id);
    }
    public class Credentials : ICredentialsRepository
    {
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;

        public Credentials(IUserService userService, IUserRepository userRepository)
        {
            this.userService = userService;
            this.userRepository = userRepository;
        }

        public async Task<Credential> GetCredentials(int id)
        {
            var credentials = new Credential();
            credentials.id_role = userRepository.GetUserRole(id);
            credentials.path_image = await userRepository.GetUserPicturePath(id);
            credentials.role_name = await userRepository.GetUserRoleName(credentials.id_role);
            return credentials;
        }
    }
}