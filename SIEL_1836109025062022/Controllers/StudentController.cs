using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
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
        [HttpGet]
        public async Task<IActionResult> StudentCurrentLevelElection()
        {
            var student_id = userService.GetUserId();
            var isStudentCoursing = await studentsRepository.IsStudentCoursing(student_id);
            var id_program = await studentsRepository.GetStudentProgramId(student_id);
            var program = await courseProgramRepository.GetCourseProgramById(id_program);
            if (!isStudentCoursing)
            {

                var model = new LevelElectionViewModel
                {
                    Levels = await levelsRepository.GetStudentLevelsByIdProgram(id_program),
                };
                ViewBag.status = isStudentCoursing;
                if (model == null)
                {
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    ViewData["studentProgram"] = program.program_name.ToString();
                    return View(model);
                }
            }
            else
            {
                ViewData["studentProgram"] = program.program_name.ToString();
                ViewBag.status = isStudentCoursing;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> StudentCurrentLevelElection(int id_level)
        {
            var id_student = userService.GetUserId();
            var current_level = id_level;


            await studentsRepository.UpdateStudentLevel(id_student, current_level);
            await studentsRepository.UpdateStudentCoursingLevel(id_student, current_level);

            return RedirectToAction("Index", "Student");



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

            var levels = await levelsRepository.GetStudentLevelsByIdProgram(id_program_selected);
            var notes = "[{'speaking':'100','writing':'100'}]";
            foreach (var level in levels)
            {
                var curriculumItem = new CurriculumAdvance()
                {
                    crlm_id_level = level.id_level,
                    crlm_id_student = student_id,
                    crlm_notes = notes,
                    crlm_start_date = DateTime.Parse("2000-07-08 23:51:33.680"),
                    crlm_end_date = DateTime.Parse("2000-07-08 23:51:33.680"),
                };
                await studentsRepository.CreateCurriculumAdvanceById(curriculumItem);
            }
            
            return RedirectToAction("Index", "Student");
        }
    
        [HttpGet]
        public async Task<IActionResult> StudentInscription()
        {
            var student_id = userService.GetUserId();

            var student = await studentsRepository.GetStudentSchoolarData(student_id);

            if (student is null)
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                var isStudentJoined = await inscriptionRepository.IsStudentJoined(student_id);

                if (!isStudentJoined)
                {
                    ViewBag.status = isStudentJoined;
                    var student_id_program = await studentsRepository.VerifyStudentProgramById(student_id);
                    InscriptionViewModel inscriptionViewModel = new InscriptionViewModel();
                    inscriptionViewModel.insc_id_student = student_id;
                    inscriptionViewModel.Schedules = await scheduleRepository.GetAllSchedules();
                    inscriptionViewModel.Modalities = await modalityRepository.GetAllModalities();
                    inscriptionViewModel.Levels = await levelsRepository.GetStudentLevelsByIdProgram(student_id_program);
                    return View(inscriptionViewModel);
                }
                else
                {
                    ViewBag.status = isStudentJoined;
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> MakeInscription(Inscription inscription)
        {
            var student_id = userService.GetUserId();
            var student_control_number = await studentsRepository.GetStudentControlNumber(student_id);
            var student_program = await studentsRepository.GetStudentProgramId(student_id);
            var path1 = "wwwroot/EdPaymentFiles";
            var path2 = "wwwroot/InstitutionPaymentFiles";
            var file_location = "EdPaymentFiles";
            var file_location2 = "InstitutionPaymentFiles";
            if (inscription.insc_id_student == student_id)
            {
                inscription.insc_file_one = await UploadFile(path1, inscription.file_one, student_control_number, file_location);
                inscription.insc_file_two = await UploadFile(path2, inscription.file_two, student_control_number, file_location2);
                DateTime date = DateTime.Now;
                inscription.insc_date_time = date;
                inscription.insc_status = 1;
                inscription.insc_id_course_program = student_program;
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
    }
}
