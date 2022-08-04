using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class StudentController : Controller
    {
        private readonly ICourseProgramRepository courseProgramRepository;
        private readonly ILevelsRepository levelsRepository;
        private readonly IStudentsRepository studentsRepository;
        private readonly IUserService userService;
        private readonly IInscriptionRepository inscriptionRepository;
        private readonly IScheduleRepository scheduleRepository;
        private readonly IModalityRepository modalityRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public StudentController(
            ICourseProgramRepository courseProgramRepository,
            ILevelsRepository levelsRepository,
            IStudentsRepository studentsRepository,
            IUserService userService,
            IInscriptionRepository inscriptionRepository,
            IScheduleRepository scheduleRepository,
            IModalityRepository modalityRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.courseProgramRepository = courseProgramRepository;
            this.levelsRepository = levelsRepository;
            this.studentsRepository = studentsRepository;
            this.userService = userService;
            this.inscriptionRepository = inscriptionRepository;
            this.scheduleRepository = scheduleRepository;
            this.modalityRepository = modalityRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var student_id = userService.GetUserId();
            var isStudent = await studentsRepository.IsStudent(student_id);

            if (!isStudent)
            {
                var studentProgram = 0;
                var modelo = new StudenIndexViewModel();
                modelo.Programs = await GetAllCoursePrograms();
                ViewData["studentProgram"] = studentProgram;
                return View(modelo);
            }
            else
            {
                var studentProgram = await studentsRepository.VerifyStudentProgramById(student_id);
                if (studentProgram == 0)
                {
                    var modelo = new StudenIndexViewModel();
                    modelo.Programs = await GetAllCoursePrograms();
                    ViewData["studentProgram"] = studentProgram;
                    return View(modelo);
                }
                else
                {
                    ViewData["studentProgram"] = studentProgram;
                    var modelo = new StudenIndexViewModel();
                    modelo.levels = await levelsRepository.GetStudentLevelsByIdProgram(studentProgram);
                    return View(modelo);
                }
            }
            
            

        }

        public IActionResult StudentData()
        {
            return View("StudentData");
        }

        public IActionResult StudentAnnouncement()
        {
            return View("StudentAnnouncement");
        }


        public IActionResult StudentGetPaymentData()
        {
            return View("StudentGetPaymentData");
        }
        [HttpGet]
        public async Task<IActionResult> StudentPersonalData()
        {
            var student_id = userService.GetUserId();

            var student = await studentsRepository.GetStudentSchoolarData(student_id);

            if (student is null)
            {
                return RedirectToAction("Index", "Student");
            }
            return View(student);
        }

        private async Task<IEnumerable<SelectListItem>> GetAllCoursePrograms()
        {
            var programs = await courseProgramRepository.GetAllCoursePrograms();
            return programs.Select(x => new SelectListItem(
                    x.program_name,
                    x.id_program.ToString()));
        }

        public async Task<IActionResult> StudentProgramElection(StudenIndexViewModel student)
        {
            var student_id = userService.GetUserId();
            int id_program_selected = student.level_id_program;
            await studentsRepository.CreateStudentProgramId(student_id, id_program_selected);
            return RedirectToAction("Index", "Student");
        }
    
        [HttpGet]
        public async Task<IActionResult> StudentInscription()
        {
            var student_id = userService.GetUserId();
            InscriptionViewModel inscriptionViewModel = new InscriptionViewModel();

            inscriptionViewModel.insc_id_student = student_id;
            inscriptionViewModel.Schedules = await scheduleRepository.GetAllSchedules();
            inscriptionViewModel.Modalities = await modalityRepository.GetAllModalities();

            return View(inscriptionViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> MakeInscription(Inscription inscription)
        {
            var student_id = userService.GetUserId();
            DateTime date =  DateTime.Now;
            inscription.insc_date_time = date;

            if (inscription.insc_id_student == student_id)
            {

                await inscriptionRepository.MakeInscription(inscription);
                return RedirectToAction("Index", "Student");
            }
            else
            {
                return RedirectToAction("Errore", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateControlNumber(StudentDataViewModel student)
        {
            await studentsRepository.UpdateControlNumber(student);
            return RedirectToAction("StudentPersonalData", "Student");
        }


    }
}
