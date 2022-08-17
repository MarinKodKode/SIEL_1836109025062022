using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ModalityController : Controller
    {
        private readonly IModalityRepository modalityRepository;
        private readonly ILevelsRepository levelsRepository;

        public ModalityController(IModalityRepository modalityRepository,
            ILevelsRepository levelsRepository)
        {
            this.modalityRepository = modalityRepository;
            this.levelsRepository = levelsRepository;
        }
        public async Task<IActionResult> Index()
        {
            var modalities = await modalityRepository.GetAllModalities();
            return View(modalities);
        }

        [HttpGet]
        public async Task<IActionResult> CreateModality()
        {
            var model = new ModalityViewModel
            {
                Levels = await levelsRepository.GetLevels(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModality(Modality modality)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existModality = await modalityRepository.ExistsModality(modality);

            if (existModality)
            {
                var model = new ModalityViewModel
                {
                    Levels = await levelsRepository.GetLevels(),
                };
                ModelState.AddModelError(nameof(modality.modality_name),
                    "Ya hay una modalidad con el mismo nombre, descripción y asignada al mismo nivel.");
                return View(model);
            }
            await modalityRepository.CreateModality(modality);
            return RedirectToAction("Index");


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
            var existModality = await modalityRepository.ExistsModality(modality);

            if (existModality)
            {
                ModelState.AddModelError(nameof(modality.modality_name),
                    "Ya hay una modalidad con el mismo nombre, descripción y asignada al mismo nivel.");
                return View();
            }
            await modalityRepository.UpdateModality(modality);
            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> DeleteModalityConfirmation(int id)
        {
            var modality = await modalityRepository.GetModalityLevelById(id);

            if (modality is null)
            {
                return RedirectToAction("WhereDoYouGo", "Home");
            }
            return View(modality);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteModality(int id_modality)
        {
            var modality = await modalityRepository.GetModalityById(id_modality);
            if (modality is null)
            {
                return RedirectToAction("404", "Home");
            }
            await modalityRepository.DeleteModalityById(id_modality);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> VerifyExistsModality(Modality modality)
        {
            var existModality = await modalityRepository.ExistsModality(modality);

            if (existModality)
            {
                return Json("Ya existe un programa con ese nombre");
            }

            return Json(true);
        }
    }
}