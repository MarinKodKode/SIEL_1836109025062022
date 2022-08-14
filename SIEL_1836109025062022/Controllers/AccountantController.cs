using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class AccountantController : Controller
    {
        private readonly IInscriptionRepository inscriptionRepository;
        private readonly IUserService userService;
        private readonly IAccountantRepository accountantRepository;
        private readonly IStatusRepository statusRepository;

        public AccountantController(IInscriptionRepository inscriptionRepository,
            IUserService userService,
            IAccountantRepository accountantRepository,
            IStatusRepository statusRepository)
        {
            this.inscriptionRepository = inscriptionRepository;
            this.userService = userService;
            this.accountantRepository = accountantRepository;
            this.statusRepository = statusRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await accountantRepository.GetInscriptionsRequests();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveConfirmation(int id)
        {
            var model = await inscriptionRepository.GetInscriptionRequestById(id);
            model.status = await statusRepository.GetStatusInscriptionList();
            if (model is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ApproveInscription(Inscription inscription)
        {
            await inscriptionRepository.ApproveInscription(inscription.insc_id_student, inscription.id_inscription, inscription.insc_status); 
            return RedirectToAction("Index");
        }
    }
}
