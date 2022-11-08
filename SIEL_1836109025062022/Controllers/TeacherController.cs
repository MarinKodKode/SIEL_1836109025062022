using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Models.Teacher;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IUserService userService;
        private readonly ICredentialsRepository credentials;
        private readonly IUserRepository userRepository;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITeachersRepository teachersRepository;

        public TeacherController(IUserService userService,
            ICredentialsRepository credentials,
            IUserRepository userRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITeachersRepository teachersRepository
            )
        {
            this.userService = userService;
            this.credentials = credentials;
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.teachersRepository = teachersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await teachersRepository.GetTeachersList();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> InsertTeacher()
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher(Teacher model)
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
                user_id_role = 3,
            };
            var isUser = await userRepository.ExistsUserEmail(model.user_personal_email);
            if (!isUser)
            {
                var resultado = await userManager.CreateAsync(user, password: "123456789");
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Redirect");
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return RedirectToAction("WhereDoYouGo", "Home");
                }
            }
            else
            {
                return RedirectToAction("e404", "Home");
            }

        }
        [HttpGet]
        public async Task<IActionResult> TeacherGroups()
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 3)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await teachersRepository.GetTeachersClasses(user_id);
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ClassesAssignedToTeacher(int id)
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await teachersRepository.GetTeachersClasses(id);
                User teacher_name = await userRepository.GetUserById(id);
                var view_teacher_name = teacher_name.user_name;
                var view_teacher_surname = teacher_name.user_surname;
                ViewData["teacher_name"] = view_teacher_name + " " +view_teacher_surname;
                var numberOfActiveClasses = await teachersRepository.GetTeacherActiveClasses(id);
                ViewData["numberOfActiveClasses"] = numberOfActiveClasses;
                var numberOfActiveStudents = await teachersRepository.GetTeacherActiveStudents(id);
                ViewData["numberOfActiveStudents"] = numberOfActiveStudents;
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> TeacherPersonalData(int id)
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await teachersRepository.GetTeacherData(id);
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> TeacherTrainingSection()
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 3)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> TeacherResources()
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 3)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                return View();
            }
        }

    }
}
