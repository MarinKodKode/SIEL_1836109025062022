using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Models.ViewModel;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class ModalityController : Controller
    {
        private readonly IModalityRepository modalityRepository;
        private readonly ILevelsRepository levelsRepository;
        private readonly IUserService userService;
        private readonly ICredentialsRepository credentials;

        public ModalityController(IModalityRepository modalityRepository,
            ILevelsRepository levelsRepository,
            IUserService userService,
            ICredentialsRepository credentials)
        {
            this.modalityRepository = modalityRepository;
            this.levelsRepository = levelsRepository;
            this.userService = userService;
            this.credentials = credentials;
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
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var levels = await levelsRepository.GetLevels();
                var modelo = levels
                    .GroupBy(x => x.program_name)
                    .Select(grupo => new IndexLevelsViewModel
                    {
                        program = grupo.Key,
                        levels = grupo.AsEnumerable()
                    }).ToList();

                return View(modelo);
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateModality()
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
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = new ModalityViewModel
                {
                    Levels = await levelsRepository.GetLevels(),
                };
                return View(model);
            }
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
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var modality = await modalityRepository.GetModalityById(id);
                if (modality is null)
                {
                    return RedirectToAction("Errore", "Home");
                }
                return View(modality);
            }
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
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);

            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var modality = await modalityRepository.GetModalityLevelById(id);

                if (modality is null)
                {
                    return RedirectToAction("WhereDoYouGo", "Home");
                }
                return View(modality);
            }
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