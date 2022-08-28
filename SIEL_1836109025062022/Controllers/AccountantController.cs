using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class AccountantController : Controller
    {
        private readonly IInscriptionRepository inscriptionRepository;
        private readonly IUserService userService;
        private readonly IAccountantRepository accountantRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IUserRepository userRepository;
        private readonly ICredentialsRepository credentials;
        private readonly ICourseProgramRepository courseProgramRepository;

        public AccountantController(IInscriptionRepository inscriptionRepository,
            IUserService userService,
            IAccountantRepository accountantRepository,
            IStatusRepository statusRepository,
            IUserRepository userRepository,
            ICredentialsRepository credentials,
            ICourseProgramRepository courseProgramRepository)
        {
            this.inscriptionRepository = inscriptionRepository;
            this.userService = userService;
            this.accountantRepository = accountantRepository;
            this.statusRepository = statusRepository;
            this.userRepository = userRepository;
            this.credentials = credentials;
            this.courseProgramRepository = courseProgramRepository;
        }

        
        public async Task<IActionResult> Index()
        {
            var id_user = userService.GetUserId();
            var urole = userRepository.GetUserRole(id_user);
            var upicture = await userRepository.GetUserPicturePath(id_user);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            var model = await accountantRepository.GetInscriptionsRequests();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> PaymentGraduatedProgram()
        {
            var id_user = userService.GetUserId();
            var urole = userRepository.GetUserRole(id_user);
            var upicture = await userRepository.GetUserPicturePath(id_user);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            var graduatedProgramId = await courseProgramRepository.GetGraduatedProgram();
            var model = await accountantRepository.GetInscriptionsRequestsByProgramId(graduatedProgramId);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UnsolvedPayment()
        {
            var id_user = userService.GetUserId();
            var urole = userRepository.GetUserRole(id_user);
            var upicture = await userRepository.GetUserPicturePath(id_user);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            //var graduatedProgramId = await courseProgramRepository.GetGraduatedProgram();
            var model = await accountantRepository.GetUnsolvedPayments();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UnauthorizedPayment()
        {
            var id_user = userService.GetUserId();
            var urole = userRepository.GetUserRole(id_user);
            var upicture = await userRepository.GetUserPicturePath(id_user);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            //var graduatedProgramId = await courseProgramRepository.GetGraduatedProgram();
            var model = await accountantRepository.UnauthorizedPayments();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AuthorizedPayment()
        {
            var id_user = userService.GetUserId();
            var urole = userRepository.GetUserRole(id_user);
            var upicture = await userRepository.GetUserPicturePath(id_user);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            //var graduatedProgramId = await courseProgramRepository.GetGraduatedProgram();
            var model = await accountantRepository.GetAuthorizedPayments();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentPlacementTest()
        {
            var id_user = userService.GetUserId();
            var urole = userRepository.GetUserRole(id_user);
            var upicture = await userRepository.GetUserPicturePath(id_user);
            var urole_name = await userRepository.GetUserRoleName(urole);
            ViewData["role"] = urole;
            ViewData["picture"] = upicture;
            ViewData["role_name"] = urole_name;
            var placementProgramId = await courseProgramRepository.GetPlacementTestId();
            var model = await accountantRepository.GetInscriptionsRequestsByProgramId(placementProgramId);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ApproveConfirmation(int id)
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 1 && credential.id_role != 2 && credential.id_role != 5)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await inscriptionRepository.GetInscriptionRequestById(id);
                model.status = await statusRepository.GetStatusInscriptionList();
                if (model is null)
                {
                    return RedirectToAction("e404", "Home");
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ApproveInscription(Inscription inscription)
        {
            await inscriptionRepository.ApproveInscription(inscription.insc_id_student, inscription.id_inscription, inscription.insc_status); 
            return RedirectToAction("Index");
        }
    }
}
