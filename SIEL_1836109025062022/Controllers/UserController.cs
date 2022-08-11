using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserService userService,
            IUserRepository userRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.userRepository = userRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                user_personal_email = model.user_personal_email,
                user_name = model.user_name,
                user_surname = model.user_surname,
                user_phone_1 = model.user_phone_1,
                user_id_institution =1,
                user_id_role = 4,
            };
            var resultado = await userManager.CreateAsync(user, password: model.user_hash_password);
            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Student");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                loginViewModel.Rememberme, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Parece que tus credenciales no son las correctas");
                return View(loginViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(StudentDataViewModel user)
        {
            await userRepository.UpdateUser(user);
            return RedirectToAction("StudentPersonalData", "Student");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserProfilePicture(StudentDataViewModel user)
        {
            var id_user = userService.GetUserId();
            var db_path = await userRepository.GetUserProfilePicturePath(id_user) ;
             DeleteExistingFile(db_path);

            var fileName = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/UsersPictures", id_user + Path.GetExtension(user.file_user_profile_picture.FileName));
            var file_name_db = System.IO.Path.Combine("/", "UsersPictures", id_user + Path.GetExtension(user.file_user_profile_picture.FileName));
            await user.file_user_profile_picture.CopyToAsync
                (new System.IO.FileStream(fileName, System.IO.FileMode.Create));
            await userRepository.UpdateUserProfilePicture(file_name_db, id_user);
            
            return RedirectToAction("StudentPersonalData", "Student");

        }

        public void DeleteExistingFile(string db_path)
        {
            string path =  System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/"+db_path);
            System.IO.File.Delete(path);
        }
    }
}
