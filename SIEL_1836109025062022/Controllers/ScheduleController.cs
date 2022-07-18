using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ScheduleController : Controller 
    {
        private readonly IScheduleRepository scheduleRepository;

        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return View(schedules);
        }

        public IActionResult CreateSchedule()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule(Schedule schedule)
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
