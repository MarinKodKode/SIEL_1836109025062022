using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Models.ViewModel;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleRepository scheduleRepository;
        private readonly ILevelsRepository levelsRepository;
        private readonly IUserService userService;
        private readonly ICredentialsRepository credentials;
        private readonly IModalityRepository modalityRepository;

        public ScheduleController(IScheduleRepository scheduleRepository,
            ILevelsRepository levelsRepository,
            IUserService userService,
            ICredentialsRepository credentials,
            IModalityRepository modalityRepository)
        {
            this.scheduleRepository = scheduleRepository;
            this.levelsRepository = levelsRepository;
            this.userService = userService;
            this.credentials = credentials;
            this.modalityRepository = modalityRepository;
        }

        //Read
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
                var schedules = await scheduleRepository.GetAllSchedules();
                return View(schedules);
            }
        }
        //Create
        [HttpGet]
        public async Task<IActionResult> CreateSchedule()
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
                var model = new ScheduleCreateViewModel
                {
                    Levels = await levelsRepository.GetLevels(),
                    Modalities = await modalityRepository.GetAllModalities()
                };
                
                return View(model);
            }
        }

    

        [HttpPost]
        public async Task<IActionResult> CreateSchedule(Schedule schedule)
        {
            if (!ModelState.IsValid){return View();}
            var exist = await scheduleRepository.ExistsSchedule(schedule);
            if (exist)
            {
                var model = new ScheduleCreateViewModel
                {
                    Levels = await levelsRepository.GetLevels(),
                };
                ModelState.AddModelError(nameof(schedule.schedule_name),
                    "Ya hay un horario con la misma información asignado al mismo nivel.");
                return View(model);
            }
            await scheduleRepository.CreateSchedule(schedule);
            return RedirectToAction("Index");
        }
        //Update
        [HttpGet]
        public async Task<IActionResult> EditSchedule(int id)
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
                var schedule = await scheduleRepository.GetSchedulebyId(id);
                if (schedule is null)
                {
                    return RedirectToAction("WhereDoYouGo", "Home");
                }
                return View(schedule);
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditSchedule(Schedule schedule)
        {
            if (!ModelState.IsValid) { return View(); }
            var exist = await scheduleRepository.ExistsSchedule(schedule);
            if (exist)
            {
                var model = new ScheduleCreateViewModel
                {
                    Levels = await levelsRepository.GetLevels(),
                };
                ModelState.AddModelError(nameof(schedule.schedule_name),
                    "Ya hay un horario con la misma información asignado al mismo nivel.");
                return View(model);
            };
            await scheduleRepository.UpdateSchedule(schedule);
            return RedirectToAction("Index");
        }
        //Delete
        public async Task<IActionResult> DeleteScheduleConfirmation(int id)
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
                var schedule = await scheduleRepository.GetSchedulebyId(id);
                if (schedule is null) { return RedirectToAction("e404", "Home"); }
                return View(schedule);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSchedule(int id_schedule)
        {
            var schedule = await scheduleRepository.GetSchedulebyId(id_schedule);
            if (schedule is null){return RedirectToAction("e404", "Home");}
            await scheduleRepository.DeleteScheduleById(id_schedule);
            return RedirectToAction("Index");
        }
        //Validations
        [HttpGet]
        public async Task<IActionResult> VerifyExistsSchedule(Schedule schedule)
        {
            var existModality = await scheduleRepository.ExistsSchedule(schedule);
            if (existModality)
            {
                return Json("Ya existe un horario con la misma información asignada al mismo nivel.");
            }
            return Json(true);
        }
        private async Task<IEnumerable<SelectListItem>> GetModalititesByLevelId(int id_level)
        {
            var modalities = await modalityRepository.GetAllModalitiesByLevel(id_level);
            return modalities.Select(x => new SelectListItem(x.modality_name, x.id_modality.ToString()));
        }
        [HttpPost]
        public async Task<IActionResult> GetModalitiesByLevel([FromBody] int schedule_level)
        {
            var user_id = userService.GetUserId();
            var modalities = await GetModalititesByLevelId(schedule_level);
            //var modalities = await modalityRepository.GetAllModalitiesByLevel(schedule_level);
            return Ok(modalities);
        }
         

    }
}
