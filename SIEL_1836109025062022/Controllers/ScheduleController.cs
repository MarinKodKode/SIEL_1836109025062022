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

        public async Task<IActionResult> Index()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return View(schedules);
        }

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
        public async Task<IActionResult> CreateSchedule(ScheduleCreateViewModel schedule)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await scheduleRepository.CreateSchedule(schedule);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditSchedule(int id)
        {
            var schedule = await scheduleRepository.GetSchedulebyId(id);
            if (schedule is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            return View(schedule);
        }

        [HttpPost]
        public async Task<ActionResult> EditSchedule(Schedule schedule)
        {
            var exist = await scheduleRepository.GetSchedulebyId(schedule.id_schedule);

            if (exist is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await scheduleRepository.UpdateSchedule(schedule);
            return RedirectToAction("Index");


        }

        public async Task<IActionResult> DeleteScheduleConfirmation(int id)
        {
            var schedule = await scheduleRepository.GetSchedulebyId(id);

            if (schedule is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSchedule(int id_schedule)
        {
            var schedule = await scheduleRepository.GetSchedulebyId(id_schedule);

            if (schedule is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await scheduleRepository.DeleteScheduleById(id_schedule);
            return RedirectToAction("Index");
        }

    }
}
