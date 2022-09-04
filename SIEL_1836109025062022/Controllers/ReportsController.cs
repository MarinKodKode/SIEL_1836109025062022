using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportsRepository reportsRepository;
        private readonly IUserService userService;
        private readonly ICredentialsRepository credentials;

        public ReportsController(IReportsRepository reportsRepository,
            IUserService userService,
            ICredentialsRepository credentials)
        {
            this.reportsRepository = reportsRepository;
            this.userService = userService;
            this.credentials = credentials;
        }
        public async Task<IActionResult> ReportsByProgram()
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
                var coursePrograms = await reportsRepository.GetAllCoursePrograms();
                return View(coursePrograms);
            }
        }
        /*[HttpPost]
        public async Task<IActionResult> ReportByProgramDetail(int id_program)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var program = await reportsRepository.GetCourseProgramById(id_program);
                if (program is null) { return RedirectToAction("WhereDoYouGo", "Home"); }
                return View(program);
            }
        }*/



        [HttpGet]
        public async Task<IActionResult> ReportByProgramDetail(int id)
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
                var courseProgram = await reportsRepository.GetCourseProgramById(id);
                if (courseProgram is null)
                {
                    return RedirectToAction("e404", "Home");
                }
                return View(courseProgram);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ReportByProgramDetail(ReportProgram reportProgram)
        {
            var courseExists = await reportsRepository.GetCourseProgramById(reportProgram.id_program);
            if (courseExists is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await reportsRepository.UpdateCourseProgrma(reportProgram);
            return RedirectToAction("Index");
        }
    }
}

