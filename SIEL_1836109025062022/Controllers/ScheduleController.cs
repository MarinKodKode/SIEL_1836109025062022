using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ScheduleController : Controller 
    {
        private readonly IScheduleRepository scheduleRepository;
        private readonly ILevelsRepository levelsRepository;

        public ScheduleController(IScheduleRepository scheduleRepository,
            ILevelsRepository levelsRepository)
        {
            this.scheduleRepository = scheduleRepository;
            this.levelsRepository = levelsRepository;
        }

        //Read
        public async Task<IActionResult> Index()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return View(schedules);
        }
        //Create
        [HttpGet]
        public async Task<IActionResult> CreateSchedule()
        {
            var model = new ScheduleCreateViewModel
            {
                Levels = await levelsRepository.GetLevels(),
            };
            return View(model);
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
            var schedule = await scheduleRepository.GetSchedulebyId(id);
            if (schedule is null)
            {
                return RedirectToAction("WhereDoYouGo", "Home");
            }
            return View(schedule);
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
            var schedule = await scheduleRepository.GetSchedulebyId(id);
            if (schedule is null){return RedirectToAction("e404", "Home");}
            return View(schedule);
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
    }
}
