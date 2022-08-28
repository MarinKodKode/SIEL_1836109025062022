using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class LevelsController : Controller
    {
        private readonly ICourseProgramRepository courseProgramRepository;
        private readonly ILevelsRepository levelsRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;
        private readonly ICredentialsRepository credentials;

        public LevelsController(
            ICourseProgramRepository courseProgramRepository,
            ILevelsRepository levelsRepository,
            IWebHostEnvironment webHostEnvironment,
            IUserService userService,
            IUserRepository userRepository,
            ICredentialsRepository credentials)
        {
            this.courseProgramRepository = courseProgramRepository;
            this.levelsRepository = levelsRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.userService = userService;
            this.userRepository = userRepository;
            this.credentials = credentials;
        }

        //READ
        //INDEX SECURED
        public async Task<IActionResult> Index()
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
                var levels = await levelsRepository.GetLevels();
                var modelo = levels
                    .GroupBy(x => x.program_name)
                    .Select(grupo => new IndexLevelsViewModel
                    {
                        program = grupo.Key,
                        levels = grupo.AsEnumerable()
                    }).ToList();

                return View(modelo);
            }
        }
        //CREATE
        //CREATE SECURED
        [HttpGet]
        public async Task<IActionResult> CreateLevel()
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
                var modelo = new LevelCreateViewModel();
                modelo.Programs = await GetAllCoursePrograms();
                return View(modelo);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateLevel(LevelCreateViewModel level)
        {
            var program = await courseProgramRepository.GetCourseProgramById(level.level_id_program);
            if (program is null)
            {
                return RedirectToAction("Errore", "Home");
            }

            if (!ModelState.IsValid)
            {
                level.Programs = await GetAllCoursePrograms();
                return View(level);
            }
            var path = "wwwroot/LevelsPictures";
            var file_location = "LevelsPictures";
            level.level_picture = await UploadFile(path, level.level_file_picture, level.level_name, file_location);
            await levelsRepository.CreateLevel(level);
            return RedirectToAction("index");
        }
        //UPDATE
        //UPDATE SECURED
        [HttpGet]
        public async Task<IActionResult> EditLevel(int id)
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
                var level = await levelsRepository.GetLevelById(id);
                if (level is null) { return RedirectToAction("WhereDoYouGo", "Home"); }
                return View(level);
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditLevel(Level level)
        {
            if (!ModelState.IsValid) { return View(); }
            var isEqual = await levelsRepository.ExistsLevel(level);
            if (isEqual)
            {
                var model = level;
                ModelState.AddModelError(nameof(level.level_name),
                    "Ya hay un nivel con el mismo nombre y descripción asignado al mismo programa de estudios.");
                return View(model);
            }
            await levelsRepository.UpdateLevel(level);
            return RedirectToAction("Index");


        }
        //DELETE
        [HttpGet]
        public async Task<IActionResult> DeleteLevelConfirmation(int id)
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
                var level = await levelsRepository.GetLevelById(id);
                if (level is null) { return RedirectToAction("WhereDoYouGo", "Home"); }
                return View(level);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLevel(int id_level)
        {
            var modality = await levelsRepository.GetLevelById(id_level);
            if (modality is null) { return RedirectToAction("404", "Home"); }
            await levelsRepository.DeleteLevelById(id_level);
            return RedirectToAction("Index");
        }
        //External resources
        private async Task<IEnumerable<SelectListItem>> GetAllCoursePrograms()
        {
            var programs = await courseProgramRepository.GetAllCoursePrograms();
            return programs.Select(x => new SelectListItem(
                    x.program_name,
                    x.id_program.ToString()));
        }
        [HttpGet]
        public async Task<IActionResult> VerifyExistsLevel(Level level)
        {
            var existLevel = await levelsRepository.ExistsLevel(level);
            if (existLevel)
            {
                return Json("Ya existe un nivel con ese nombre");
            }
            return Json(true);
        }
        public async Task<string> UploadFile(string path, IFormFile file, string file_name, string file_location)
        {
            var fileName = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                path, file_name + Path.GetExtension(file.FileName));
            var file_name_db = System.IO.Path.
                Combine("/", file_location,
                file_name + Path.GetExtension(file.FileName));

            await file.CopyToAsync(new System.IO.FileStream(
                fileName, System.IO.FileMode.Create));
            return file_name_db;

        }
        [HttpPost]
        public async Task<IActionResult> UpdateLevelPicture(Level level)
        {
            var level_name = level.level_name;
            var db_path = await levelsRepository.GetLevelPicturePath(level.id_level);
            DeleteExistingFile(db_path);

            var fileName = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/LevelsPictures", level_name + Path.GetExtension(level.level_file_picture.FileName));
            var file_name_db = System.IO.Path.Combine("/", "LevelsPictures", level_name + Path.GetExtension(level.level_file_picture.FileName));
            await level.level_file_picture.CopyToAsync
                (new System.IO.FileStream(fileName, System.IO.FileMode.Create));
            await levelsRepository.UpdateLevelPicture(file_name_db, level.id_level);

            return RedirectToAction("StudentPersonalData", "Student");

        }
        public void DeleteExistingFile(string db_path)
        {
            string path = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/" + db_path);
            System.IO.File.Delete(path);
        }
    }
}