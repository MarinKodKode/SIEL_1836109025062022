using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class AccountantController : Controller
    {
        private readonly IInscriptionRepository inscriptionRepository;

        public AccountantController(IInscriptionRepository inscriptionRepository)
        {
            this.inscriptionRepository = inscriptionRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await inscriptionRepository.GetInscriptionList();
            return View(model);
        }
    }
}
