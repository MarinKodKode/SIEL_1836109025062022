using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ModalityController : Controller
    {
        private readonly IModalityRepository modalityRepository;

        public ModalityController(IModalityRepository modalityRepository)
        {
            this.modalityRepository = modalityRepository;
        }
        public async Task<IActionResult> Index()
        {
            var modalities = await modalityRepository.GetAllModalities();
            return View(modalities);
        }

        public IActionResult CreateModality()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateModality(Modality modality)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await modalityRepository.CreateModality(modality);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistsCourseProgram(string modality_name)
        {
            var existePrograma = await modalityRepository.ExistsModality(modality_name);

            if (existePrograma)
            {
                return Json("Ya existe un programa con ese nombre");
            }

            return Json(true);
        }
    }
}
