using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;

namespace SIEL_1836109025062022.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<Student> userManager;
        private readonly SignInManager<Student> signInManager;

        public StudentController(UserManager<Student> userManager,
            SignInManager<Student> signInManager)
        {

            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
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
        public IActionResult StudentPersonalData()
        {
            return View("StudentPersonalData");
        }

        public IActionResult StudentRegister()
        {
            return View("StudentRegister");
        }
        [HttpPost]
        public async Task<IActionResult> StudentRegister(StudentViewModel model)
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
    }
}
