using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<Student> userManager;
        private readonly SignInManager<Student> signInManager;
        private readonly ICourseProgramRepository courseProgramRepository;
        private readonly ILevelsRepository levelsRepository;
        private readonly IStudentsRepository studentsRepository;
        private readonly IUserService userService;

        public StudentController(UserManager<Student> userManager,
            SignInManager<Student> signInManager,
            ICourseProgramRepository courseProgramRepository,
            ILevelsRepository levelsRepository,
            IStudentsRepository studentsRepository,
            IUserService userService )
        {

            this.signInManager = signInManager;
            this.courseProgramRepository = courseProgramRepository;
            this.levelsRepository = levelsRepository;
            this.studentsRepository = studentsRepository;
            this.userService = userService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var student_id = userService.GetUserId();
            var studentProgram = await studentsRepository.VerifyStudentProgramById(student_id);
            

            if (studentProgram == 0)
            {
                var modelo = new StudenIndexViewModel();
                modelo.Programs = await GetAllCoursePrograms();
                ViewData["studentProgram"] = studentProgram;
                return View(modelo);
            }
            else
            {
                ViewData["studentProgram"] = studentProgram;

                var modelo = new StudenIndexViewModel();
                modelo.levels = await levelsRepository.GetStudentLevelsByIdProgram(studentProgram);

                return View(modelo);
            }

        }

        public IActionResult StudentRegister()
        {
            return View();
        }

        public IActionResult StudentData()
        {
            return View("StudentData");
        }

        public IActionResult StudentAnnouncement()
        {
            return View("StudentAnnouncement");
        }

        public IActionResult StudentInscription()
        {
            return View("StudentInscription");
        }

        public IActionResult StudentGetPaymentData()
        {
            return View("StudentGetPaymentData");
        }
        [HttpGet]
        public async Task<IActionResult> StudentPersonalData()
        {
            var student_id = userService.GetUserId();
            var student = await studentsRepository.GetStudentById(student_id);
            if (student is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            return View(student);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var student = new Student() { 
                stdt_personal_email = model.stdt_personal_email,
                stdt_name = model.stdt_name,
                stdt_surname = model.stdt_surname,
                stdt_phone_1 = model.stdt_phone_1,
                //stdt_phone_2 = model.stdt_phone_2,
                //stdt_institutional_email = model.stdt_institutional_email,
                //stdt_avatar = model.stdt_avatar,
                //stdt_id_institution = model.stdt_id_institution,
                //stdt_isLogged = model.stdt_isLogged,
                //stdt_id_class = model.stdt_id_class,
                //stdt_id_program = model.stdt_id_program,
                //stdt_age = model.stdt_age,
                //stdt_normalized_i_email = model.stdt_normalized_i_email,
                //stdt_control_number = model.stdt_control_number,
            };
            var resultado = await userManager.CreateAsync(student, password:model.stdt_hash_password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(student, isPersistent: true);
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var resultado = await signInManager.PasswordSignInAsync(
                model.Email,model.Password,model.Rememberme, lockoutOnFailure : false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Algun dato es incorrecto");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> StudentLogout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index","Home");
        }

        private async Task<IEnumerable<SelectListItem>> GetAllCoursePrograms()
        {
            var programs = await courseProgramRepository.GetAllCoursePrograms();
            return programs.Select(x => new SelectListItem(
                    x.program_name,
                    x.id_program.ToString()));
        }

        public async Task<IActionResult> StudentProgramElection(StudenIndexViewModel student)
        {
            var student_id = userService.GetUserId();
            int id_program_selected = student.level_id_program;
            await studentsRepository.UpdateStudentProgramId(student_id, id_program_selected);
            return RedirectToAction("Index", "Student");
        }
    
    

    
    }
}
