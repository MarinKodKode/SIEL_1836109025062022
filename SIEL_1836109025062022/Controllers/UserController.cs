using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
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
        private readonly ICredentialsRepository credentials;

        public UserController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserService userService,
            IUserRepository userRepository,
            IWebHostEnvironment webHostEnvironment,
            ICredentialsRepository credentials)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.userRepository = userRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.credentials = credentials;
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
                user_id_institution = 1,
                user_id_role = 4,
            };
            var isUser = await userRepository.ExistsUserEmail(model.user_personal_email);
            if (!isUser)
            {
                var resultado = await userManager.CreateAsync(user, password: model.user_hash_password);
                if (resultado.Succeeded)

                {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Redirect");

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
            else
            {
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
            //var user_id = userService.GetUserId();
            if (result.Succeeded)
            {

                return RedirectToAction("Index", "Redirect");
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
        //This method works only for the student module
        public async Task<IActionResult> UpdateUser(StudentDataViewModel user)
        {
            await userRepository.UpdateUser(user);
            return RedirectToAction("StudentPersonalData", "Student");
        }

        [HttpPost]
        //Method only available for user key #4 or student
        public async Task<IActionResult> UpdateUserProfilePicture(StudentDataViewModel user)
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            var id_user = userService.GetUserId();
            var db_path = await userRepository.GetUserProfilePicturePath(id_user);
            DeleteExistingFile(db_path);

            var fileName = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/UsersPictures", id_user + Path.GetExtension(user.file_user_profile_picture.FileName));
            var file_name_db = System.IO.Path.Combine("/", "UsersPictures", id_user + Path.GetExtension(user.file_user_profile_picture.FileName));
            await user.file_user_profile_picture.CopyToAsync
                (new System.IO.FileStream(fileName, System.IO.FileMode.Create));
            await userRepository.UpdateUserProfilePicture(file_name_db, id_user);
            if (credential.id_role == 4)
            {
                return RedirectToAction("StudentPersonalData", "Student");
            }
            else if (credential.id_role == 3)
            {
                return RedirectToAction("TeacherGroups", "Teacher");
            }
            else
            {
                return RedirectToAction("Index", "Levels");
            }


        }

        public void DeleteExistingFile(string db_path)
        {
            string path = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/" + db_path);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserPersonalData()
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);
            //User key #4 is student, a method for data update has been already defined
            if (credential.id_role != 1 && credential.id_role != 2 && credential.id_role != 3 && credential.id_role != 5)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await userRepository.GetUserById(user_id);
                return View(model);
            }
        }
        [HttpPost]
        //This method works only for the student module
        public async Task<IActionResult> UpdateUserPersonalData(User user)
        {
            await userRepository.UpdateUser(user);
            return RedirectToAction("TeacherGroups", "Teacher");
        }
    }
}