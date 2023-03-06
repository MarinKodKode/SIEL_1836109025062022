using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SIEL_1836109025062022.Models.Classes;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;
        private readonly ICredentialsRepository credentials;
        private readonly ITeachersRepository teachersRepository;
        private readonly IScheduleRepository scheduleRepository;
        private readonly IInscriptionRepository inscriptionRepository;
        private readonly IClassesRepository classesRepository;
        private readonly IStudentsRepository studentsRepository;

        public ClassesController(IUserService userService,
            IUserRepository userRepository,
            ICredentialsRepository credentials,
            ITeachersRepository teachersRepository,
            IScheduleRepository scheduleRepository,
            IInscriptionRepository inscriptionRepository,
            IClassesRepository classesRepository,
            IStudentsRepository studentsRepository
            )
        {
            this.userService = userService;
            this.userRepository = userRepository;
            this.credentials = credentials;
            this.teachersRepository = teachersRepository;
            this.scheduleRepository = scheduleRepository;
            this.inscriptionRepository = inscriptionRepository;
            this.classesRepository = classesRepository;
            this.studentsRepository = studentsRepository;
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
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await classesRepository.GetClasses();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateClass(int id)
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
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                //Model datas
                var _inscriptions_count = await inscriptionRepository.CountInscriptionByIdSchedule(id);
                var _noClassCount = await inscriptionRepository.CountInscriptionByIdScheduleWithNoClass(id);
                var classData = await scheduleRepository.GetSchedulebyId(id);
                var model = new ClassCreateViewModel
                {
                    program_name = classData.program_name,
                    level_name = classData.level_name,
                    modality_name = classData.modality_name,
                    schedule_name = classData.schedule_name + " - " + classData.schedule_description,
                    Teachers = await teachersRepository.GetAllTeachers(),
                    Adm_Institution = await teachersRepository.GetAllAdmInstitution(),
                    inscrptions_count = _inscriptions_count,
                    noClassCount = _noClassCount,
                    id_schedule = classData.id_schedule
                };
                return View(model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateClass(ClassCreateViewModel NewClass)
        {
            if (NewClass is null)
            {
                return RedirectToAction("e404", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var classes = await classesRepository.CreateClass(NewClass);
            NewClass.assignedClass = classes;
            await classesRepository.AssignClassToStudents(NewClass);
            return RedirectToAction("ListsStudentsSchedule", "Lists", new { id = NewClass.group_id_schedule });
        }

        [HttpGet]
        public async Task<IActionResult> EditClass(int id)
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
                //Credentials
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var class_to_update = await classesRepository.GetClassById(id);
                class_to_update.Teachers = await teachersRepository.GetAllTeachers();
                class_to_update.Adm_Institution = await teachersRepository.GetAllAdmInstitution();
                var model = class_to_update;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditClass(ClassCreateViewModel class_to_update)
        {
            //if (!ModelState.IsValid) { return View(); }
            //var isEqual = await levelsRepository.ExistsLevel(level);
            //if (isEqual)
            //{
            //    var model = level;
            //    ModelState.AddModelError(nameof(level.level_name),
            //        "Ya hay un nivel con el mismo nombre y descripción asignado al mismo programa de estudios.");
            //    return View(model);
            //}
            await classesRepository.UpdateClass(class_to_update);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteClassConfirmation(int id)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (credential.id_role != 1 && credential.id_role != 2) { return RedirectToAction("e404", "Home"); }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var class_to_delete = await classesRepository.GetClassById(id);
                if (class_to_delete is null) { return RedirectToAction("e404", "Home"); }
                var model = class_to_delete;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteClass(ClassCreateViewModel class_delete)
        {
            var class_to_delete = await classesRepository.GetClassById(class_delete.id_class);
            if (class_to_delete is null) { return RedirectToAction("e404", "Home"); }
            await classesRepository.ResetClassToStudents(class_delete.id_class);
            await classesRepository.DeleteClass(class_delete.id_class);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentsJoinedToClass(int id)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (credential.id_role != 1 && credential.id_role != 2 && credential.id_role != 3) { return RedirectToAction("e404", "Home"); }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                ViewData["id_class"] = id;
                var studentsJoinedToClass = await studentsRepository.GetStudentsInformationByIdClass(id);
                var model = studentsJoinedToClass;
                return View("StudentsJoinedToClass", model);
            }
        }

        public async Task<IActionResult> PrintStudentsList(int id)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (credential.id_role != 1 && credential.id_role != 2 && credential.id_role != 3) { return RedirectToAction("e404", "Home"); }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var studentsJoinedToClass = await studentsRepository.GetStudentsInformationByIdClass(id);
                var model = studentsJoinedToClass;
                return new ViewAsPdf("PrintStudentsList", model)
                {

                    FileName = $"Testing Rotativa.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4
                };
            }
        }
    }


}