using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Models.ViewModel;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ListsController : Controller
    {
        private readonly ILevelsRepository levelsRepository;
        private readonly ICourseProgramRepository courseProgramRepository;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;
        private readonly ICredentialsRepository credentials;
        private readonly IModalityRepository modalityRepository;
        private readonly IScheduleRepository scheduleRepository;
        private readonly IListRepository listRepository;

        public ListsController(ILevelsRepository levelsRepository,
            ICourseProgramRepository courseProgramRepository,
            IUserService userService,
            IUserRepository userRepository,
            ICredentialsRepository credentials,
            IModalityRepository modalityRepository,
            IScheduleRepository scheduleRepository,
            IListRepository listRepository)
        {
            this.levelsRepository = levelsRepository;
            this.courseProgramRepository = courseProgramRepository;
            this.userService = userService;
            this.userRepository = userRepository;
            this.credentials = credentials;
            this.modalityRepository = modalityRepository;
            this.scheduleRepository = scheduleRepository;
            this.listRepository = listRepository;
        }
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
        public async Task<IActionResult> ListsModality(int id)
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
                var schedules = await scheduleRepository.GetAllSchedulesByLevel(id);
                var model = schedules
                    .GroupBy(x => x.modality_name)
                    .Select(group => new ListsScheduleViewModel
                    {
                        modality = group.Key.ToString(),
                        schedules = group.AsEnumerable()
                    }).ToList();
                //var model = await modalityRepository.GetAllModalitiesByLevel(id);
                var level = await levelsRepository.GetLevelById(id);
                if (model == null)
                {
                    return RedirectToAction("e404", "Home");
                }
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                ViewData["level_name"] = level.level_name;
                ViewData["id_level"] = level.id_level;
                return View(model);
            }
        }
        //Read
        public async Task<IActionResult> ListsModalitySchedules(int id)
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
                var modality = await modalityRepository.GetModalityById(id);
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                ViewData["id_modality"] = modality.id_modality;
                ViewData["modality_name"] = modality.modality_name;
                var schedules = await scheduleRepository.GetAllSchedulesByModality(id);
                return View(schedules);
            }
        }
       //Lists of students
        public async Task<IActionResult> ListsStudentsSchedule(int id)
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
                ViewData["id"] = id;
                var students = await listRepository.GetStudentsListBySchedule(id);
                var model = students;
                return View(model);
            }
        }
    }
}
