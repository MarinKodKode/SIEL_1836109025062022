﻿using Microsoft.AspNetCore.Authentication;
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
        private readonly IUserRepository userRepository;

        public StudentController(
            ICourseProgramRepository courseProgramRepository,
            ILevelsRepository levelsRepository,
            IStudentsRepository studentsRepository,
            IUserService userService,
            IInscriptionRepository inscriptionRepository,
            IScheduleRepository scheduleRepository,
            IModalityRepository modalityRepository,
            IWebHostEnvironment webHostEnvironment,
            IUserRepository userRepository)
        {
            this.courseProgramRepository = courseProgramRepository;
            this.levelsRepository = levelsRepository;
            this.studentsRepository = studentsRepository;
            this.userService = userService;
            this.inscriptionRepository = inscriptionRepository;
            this.scheduleRepository = scheduleRepository;
            this.modalityRepository = modalityRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.userRepository = userRepository;
        }
       
        public async Task<IActionResult> Index()
        {
            var student_id = userService.GetUserId();
            var urole =  userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            //var isStudent  =  await studentsRepository.IsStudent(student_id);

            
            if (urole == 4) { 
                    var isStudent = await studentsRepository.IsStudent(student_id);
                    if (!isStudent)
                    {
                        var studentProgram = 0;
                        var modelo = new StudenIndexViewModel();
                        modelo.Programs = await GetAllCoursePrograms();
                        ViewData["studentProgram"] = studentProgram;
                        ViewData["role"] = urole;
                        ViewData["picture"] = upicture;
                        ViewData["role_name"] = urole_name;
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
                            ViewData["role"] = urole;
                            ViewData["picture"] = upicture;
                            ViewData["role_name"] = urole_name;
                            return View(modelo);
                        }
                        else
                        {
                            ViewData["studentProgram"] = studentProgram;
                            var modelo = new StudenIndexViewModel();
                            modelo.levels = await levelsRepository.GetStudentLevelsByIdProgram(studentProgram);
                            ViewData["role"] = urole;
                            ViewData["picture"] = upicture;
                            ViewData["role_name"] = urole_name;
                            return View(modelo);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("e404", "Home");
                }
            

        }
        public async Task<IActionResult> Begin()
        {
            var id_user = userService.GetUserId();
            var isStudent = await studentsRepository.IsStudent(id_user);
            if (!isStudent)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Student");
            }
            
        }
        public async Task<IActionResult> NewJoiningSteps(){return View();}
        public async Task<IActionResult> PlacementTestSteps(){return View();}
        public async Task<IActionResult> GraduatedProgramSteps(){return View();}
        [HttpGet]
        public async Task<IActionResult> StudentCurrentLevelElection()
        {

            var student_id = userService.GetUserId();
            var student = await studentsRepository.GetStudentSchoolarData(student_id);
            if (student is null)
            {
                return RedirectToAction("Begin", "Student");
            }
            var isStudentCoursing = await studentsRepository.IsStudentCoursing(student_id);
            var id_program = await studentsRepository.GetStudentProgramId(student_id);
            var program = await courseProgramRepository.GetCourseProgramById(id_program);

            var urole = userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);

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
                    ViewData["role"] = urole;
                    ViewData["picture"] = upicture;
                    ViewData["role_name"] = urole_name;
                    return View(model);
                }
            }
            else
            {
                ViewData["studentProgram"] = program.program_name.ToString();
                ViewBag.status = isStudentCoursing;
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
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
        public IActionResult AnnouncementList()
        {
            return View("AnnouncementList");
        } 
        public async Task<IActionResult> StudentGetPaymentData()
        {
            var id = userService.GetUserId();
            var urole = userRepository.GetUserRole(id);
            var upicture = await userRepository.GetUserPicturePath(id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            return View("StudentGetPaymentData");
        }
        public async Task<IActionResult> StudentInstitutionPayments()
        {
            var id = userService.GetUserId();
            var urole = userRepository.GetUserRole(id);
            var upicture = await userRepository.GetUserPicturePath(id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            return View("StudentInstitutionPayments");
        }
        [HttpGet]
        public async Task<IActionResult> PaymentDataProgram()
        {
            var id = userService.GetUserId();
            var urole = userRepository.GetUserRole(id);
            var upicture = await userRepository.GetUserPicturePath(id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            return View("PaymentDataProgram");
        }
        [HttpGet]
        public async Task<IActionResult> StudentPersonalData()
        {
            var student_id = userService.GetUserId();

            var student = await studentsRepository.GetStudentSchoolarData(student_id);

            if (student is null)
            {
                return RedirectToAction("Begin", "Student");
            }
            var urole = userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            return View(student);
        }
        private async Task<IEnumerable<SelectListItem>> GetAllCoursePrograms()
        {
            var programs = await courseProgramRepository.GetAllCoursePrograms();
            return programs.Select(x => new SelectListItem(
                    x.program_name,
                    x.id_program.ToString()));
        }
        [HttpPost]
        public async Task<IActionResult> StudentProgramElection(StudenIndexViewModel student)
        {
            var student_id = userService.GetUserId();
            int id_program_selected = student.level_id_program;
            var isStudent = await studentsRepository.IsStudent(student_id);

            if (!isStudent)
            {
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
            else
            {
                return RedirectToAction("Index", "Student");
            }
        }
        [HttpGet]
        public async Task<IActionResult> StudentInscription()
        {
            var student_id = userService.GetUserId();

            var student = await studentsRepository.GetStudentSchoolarData(student_id);

            if (student is null)
            {
                return RedirectToAction("Begin", "Student");
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
                    var urole = userRepository.GetUserRole(student_id);
                    var upicture = await userRepository.GetUserPicturePath(student_id);
                    var urole_name = await userRepository.GetUserRoleName(urole);
                    ViewData["role"] = urole;
                    ViewData["picture"] = upicture;
                    ViewData["role_name"] = urole_name;
                    return View(inscriptionViewModel);
                }
                else
                {
                    ViewBag.status = isStudentJoined;
                    var urole = userRepository.GetUserRole(student_id);
                    var upicture = await userRepository.GetUserPicturePath(student_id);
                    var urole_name = await userRepository.GetUserRoleName(urole);
                    ViewData["role"] = urole;
                    ViewData["picture"] = upicture;
                    ViewData["role_name"] = urole_name;
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
        [HttpGet]
        public async Task<IActionResult> PlacementTest()
        {
            var id = userService.GetUserId();
            var urole = userRepository.GetUserRole(id);
            var upicture = await userRepository.GetUserPicturePath(id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            int ? isStudentJoined = await inscriptionRepository.IsStudentInt(id);
            
            if (isStudentJoined == 0)
            {
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                var model = new PlacementTest();
                model.insc_id_student = id;
                ViewBag.status = isStudentJoined;
                model.insc_id_course_program = await courseProgramRepository.GetPlacementTestId();
                model.insc_id_level = await levelsRepository.GetLevelMinimunLevel(model.insc_id_course_program);
                var pltId = model.insc_id_level;
                model.Schedules = await scheduleRepository.GetAllSchedulesByLevel(pltId);
                return View(model);
            }
            else
            {
                ViewBag.status = isStudentJoined;
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegisterPlacementTest(PlacementTest placementTest)
        {
            var student_id = userService.GetUserId();
            var urole = userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            await studentsRepository.CreateStudentProgramId(student_id, placementTest.insc_id_course_program);
            //Getting variables ready
            var student_control_number = await studentsRepository.GetStudentControlNumber(student_id);
            var control_number = "fileponde";
            if (student_control_number == null)
            {
                control_number = "PlTs" + student_id;
            }
            else
            {
                control_number = student_control_number.ToString();
            }
            var student_program = placementTest.insc_id_course_program;
            var student_level = await levelsRepository.GetLevelMinimunLevel(student_program);
            var path1 = "wwwroot/EdPaymentFiles";
            var path2 = "wwwroot/InstitutionPaymentFiles";
            var file_location = "EdPaymentFiles";
            var file_location2 = "InstitutionPaymentFiles";
            //Making inscription
            if (placementTest.insc_id_student == student_id)
            {
                placementTest.insc_file_one = await UploadFile(path1, placementTest.file_one, control_number, file_location);
                placementTest.insc_file_two = await UploadFile(path2, placementTest.file_two, control_number, file_location2);
                DateTime date = DateTime.Now;
                placementTest.insc_date_time = date;
                placementTest.insc_status = 1;
                placementTest.insc_id_course_program = student_program;
                placementTest.insc_id_level = student_level;
                await inscriptionRepository.MakePlacementTestRequest(placementTest);
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return RedirectToAction("Index", "Student");


            }
            else
            {
                return RedirectToAction("Errore", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> NewJoining()
        {
            var student_id = userService.GetUserId();
            var isStudentJoined = await inscriptionRepository.IsStudentJoined(student_id);
            var urole = userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            var id_level = await levelsRepository.GetLevelMinimunLevel(1);
            var student = await studentsRepository.GetStudentSchoolarData(student_id);

            /*if (student is null)
            {
                return RedirectToAction("Index", "Student");
            }*/
            if (!isStudentJoined)
            {
                ViewBag.status = isStudentJoined;
                //var student_id_program = await studentsRepository.VerifyStudentProgramById(student_id);
                var student_id_program = 1;
                InscriptionViewModel inscriptionViewModel = new InscriptionViewModel();
                inscriptionViewModel.insc_id_student = student_id;
                inscriptionViewModel.Schedules = await scheduleRepository.GetAllSchedulesByLevel(id_level);
                inscriptionViewModel.Modalities = await modalityRepository.GetAllModalitiesByLevel(id_level);
                inscriptionViewModel.insc_id_level = id_level;
                
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return View(inscriptionViewModel);
            }
            else
            {
                ViewBag.status = isStudentJoined;
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> MakeInscriptionNewJoining(Inscription inscription)
        {
            //Getting credentials
            var student_id = userService.GetUserId();
            var urole = userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            var stdt_program = 1;
            //Creating student curriculum advance
            await studentsRepository.CreateStudentProgramId(student_id, stdt_program);
            var levels = await levelsRepository.GetStudentLevelsByIdProgram(stdt_program);
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


            //Getting variables ready
            var student_control_number = await studentsRepository.GetStudentControlNumber(student_id);
            var control_number="fileponde";
            if(student_control_number == null)
            {
                 control_number = "NI" + student_id;
            }
            else
            {
                 control_number = student_control_number.ToString();
            }
            var student_program = stdt_program;
            var student_level = await levelsRepository.GetLevelMinimunLevel(student_program);
            var path1 = "wwwroot/EdPaymentFiles";
            var path2 = "wwwroot/InstitutionPaymentFiles";
            var file_location = "EdPaymentFiles";
            var file_location2 = "InstitutionPaymentFiles";
            //Making inscription
            if (inscription.insc_id_student == student_id)
            {
                inscription.insc_file_one = await UploadFile(path1, inscription.file_one, control_number, file_location);
                inscription.insc_file_two = await UploadFile(path2, inscription.file_two, control_number, file_location2);
                DateTime date = DateTime.Now;
                inscription.insc_date_time = date;
                inscription.insc_status = 1;
                inscription.insc_id_course_program = student_program;
                inscription.insc_id_level = student_level;
                await inscriptionRepository.MakeInscription(inscription);
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return RedirectToAction("Index", "Student");
            }
            else
            {
                return RedirectToAction("Errore", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GraduatedProgramInscription()
        {

            var student_id = userService.GetUserId();
            var isStudentJoined = await inscriptionRepository.IsStudentJoined(student_id);
            var urole = userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            var student_id_program = await courseProgramRepository.GetGraduatedProgram();
            var id_level = await levelsRepository.GetLevelMinimunLevel(student_id_program);
            var student = await studentsRepository.GetStudentSchoolarData(student_id);

            if (!isStudentJoined)
            {
                ViewBag.status = isStudentJoined;
                //var student_id_program = await studentsRepository.VerifyStudentProgramById(student_id);
                var program = student_id_program;
                InscriptionViewModel inscriptionViewModel = new InscriptionViewModel();
                inscriptionViewModel.insc_id_student = student_id;
                inscriptionViewModel.Schedules = await scheduleRepository.GetAllSchedulesByLevel(id_level);
                inscriptionViewModel.Modalities = await modalityRepository.GetAllModalitiesByLevel(id_level);
                inscriptionViewModel.insc_id_level = id_level;

                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return View(inscriptionViewModel);
            }
            else
            {
                ViewBag.status = isStudentJoined;
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GraduatedProgramInscription(InscriptionViewModel inscription)
        {
            //Getting credentials
            var student_id = userService.GetUserId();
            var urole = userRepository.GetUserRole(student_id);
            var upicture = await userRepository.GetUserPicturePath(student_id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            var stdt_program = await courseProgramRepository.GetGraduatedProgram(); ;
            //Creating student curriculum advance
            await studentsRepository.CreateStudentProgramId(student_id, stdt_program);
            var levels = await levelsRepository.GetStudentLevelsByIdProgram(stdt_program);
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


            //Getting variables ready
            var student_control_number = inscription.control_number;
            var control_number = "fileponde";
            if (student_control_number == null)
            {
                control_number = "NI" + student_id;
            }
            else
            {
                control_number = student_control_number.ToString();
            }
            var student_program = stdt_program;
            var student_level = await levelsRepository.GetLevelMinimunLevel(student_program);
            var path1 = "wwwroot/EdPaymentFiles";
            var path2 = "wwwroot/InstitutionPaymentFiles";
            var file_location = "EdPaymentFiles";
            var file_location2 = "InstitutionPaymentFiles";
            //Making inscription
            if (inscription.insc_id_student == student_id)
            {
                inscription.insc_file_one = await UploadFile(path1, inscription.file_one, control_number, file_location);
                inscription.insc_file_two = await UploadFile(path2, inscription.file_two, control_number, file_location2);
                DateTime date = DateTime.Now;
                inscription.insc_date_time = date;
                inscription.insc_status = 1;
                inscription.insc_id_course_program = student_program;
                inscription.insc_id_level = student_level;
                await inscriptionRepository.MakeInscription(inscription);
                ViewData["role"] = urole;
                ViewData["picture"] = upicture;
                ViewData["role_name"] = urole_name;
                return RedirectToAction("Index", "Student");


            }
            else
            {
                return RedirectToAction("Errore", "Home");
            }
        }
        private async Task<IEnumerable<SelectListItem>> GetAllSchedulesByModality(int schedule_modality)
        {
            var schedules = await scheduleRepository.GetAllSchedulesByModality(schedule_modality);
            return schedules.Select(x => new SelectListItem(x.schedule_description, x.id_schedule.ToString()));
        }
        [HttpPost]
        public async Task<IActionResult> GetScheduleByModalityJson([FromBody] int schedule_modality)
        {
            var user_id = userService.GetUserId();
            var schedules = await GetAllSchedulesByModality(schedule_modality);
            //var modalities = await modalityRepository.GetAllModalitiesByLevel(schedule_level);
            return Ok(schedules);
        }
        [HttpGet]
        public async Task<IActionResult> directions_for_institution_payment(){
            var id = userService.GetUserId();
            var urole = userRepository.GetUserRole(id);
            var upicture = await userRepository.GetUserPicturePath(id);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            return View();  
        }
          
        
    }

}
