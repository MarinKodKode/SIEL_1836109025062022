using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class Redirect : Controller
    {
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;

        public Redirect(IUserService userService, IUserRepository userRepository)
        {
            this.userService = userService;
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var id = userService.GetUserId();
            var role = await userRepository.GetAsyncUserRole(id);

            if (role == 5)
            {
                return RedirectToAction("Index", "Accountant");
            }
            else if (role == 4)
            {
                return RedirectToAction("Begin", "Student");
            }
            else if (role == 2)
            {
                return RedirectToAction("Index", "Levels");
            }
            else
            {
                return RedirectToAction("e404", "Home");
            }
        }
    }
}