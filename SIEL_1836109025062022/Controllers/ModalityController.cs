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
                return Json("Ya existe una modalidad con ese nombre");
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> EditModality(int id)
        {
            var modality = await modalityRepository.GetModalityById(id);
            if (modality is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            return View(modality);
        }

        [HttpPost]
        public async Task<ActionResult> EditModality(Modality modality)
        {
            var modalityExists = await modalityRepository.GetModalityById(modality.id_modality);

            if (modalityExists is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await modalityRepository.UpdateModality(modality);
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> DeleteModalityConfirmation(int id)
        {
            var modality = await modalityRepository.GetModalityById(id);

            if (modality is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            return View(modality);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteModality(int id_modality)
        {
            var levelProgram = await modalityRepository.GetModalityById(id_modality);

            if (levelProgram is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await modalityRepository.DeleteModalityById(id_modality);
            return RedirectToAction("Index");
        }

    }
}
