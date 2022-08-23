using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class CourseProgramController : Controller
    {
        private readonly ICourseProgramRepository courseProgramRepository;
        private readonly IUserService userService;
        private readonly ICredentialsRepository credentials;

        public CourseProgramController(ICourseProgramRepository courseProgramRepository,
            IUserService userService,
            ICredentialsRepository credentials)
        {
            this.courseProgramRepository = courseProgramRepository;
            this.userService = userService;
            this.credentials = credentials;
        }

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
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var coursePrograms = await courseProgramRepository.GetAllCoursePrograms();
                return View(coursePrograms);
            } 
        }
        public async Task<IActionResult> CreateCourseProgram()
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
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseProgram(CourseProgram courseProgram)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existePrograma = await courseProgramRepository.ExistsCourseProgram(courseProgram.program_name);

            if (existePrograma)
            {
                ModelState.AddModelError(nameof(courseProgram.program_name), 
                    "Ya hay un programa con el mismo nombre");
                return View();
            }
           await courseProgramRepository.CreateCourseProgram(courseProgram);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditCourseProgram(int id)
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
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var courseProgram = await courseProgramRepository.GetCourseProgramById(id);
                if (courseProgram is null)
                {
                    return RedirectToAction("e404", "Home");
                }
                return View(courseProgram);
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditCourseProgram(CourseProgram courseProgram)
        {
            var courseExists = await courseProgramRepository.GetCourseProgramById(courseProgram.id_program);
            if(courseExists is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await courseProgramRepository.UpdateCourseProgrma(courseProgram);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCourseProgramConfirmation(int id)
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
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var courseProgram = await courseProgramRepository.GetCourseProgramById(id);

                if (courseProgram is null)
                {
                    return RedirectToAction("e404", "Home");
                }
                return View(courseProgram);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourseProgram(int id_program)
        {
            var courseProgram = await courseProgramRepository.GetCourseProgramById(id_program);

            if (courseProgram is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await courseProgramRepository.DeleteCourseProgramById(id_program);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistsCourseProgram(string program_name)
        {
            var existePrograma = await courseProgramRepository.ExistsCourseProgram(program_name);
            if (existePrograma)
            {
                return Json("Ya existe un programa con ese nombre");
            }
            return Json(true);
        }



    }
}
