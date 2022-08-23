using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Credentials;
using SIEL_1836109025062022.Services;
using System.Web;
using System.Drawing;
using System.IO;
namespace SIEL_1836109025062022.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IUserService userService;
        private readonly ICredentialsRepository credentials;
        private readonly IAnnouncementRepository announcementRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AnnouncementController(IUserService userService,
            ICredentialsRepository credentials,
            IAnnouncementRepository announcementRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userService = userService;
            this.credentials = credentials;
            this.announcementRepository = announcementRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
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
                var model = await announcementRepository.GetAnnouncements();
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                return View(model);
            }
        }

        public async Task<IActionResult> AnnouncementList()
        {
            var user_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(user_id);
            var model = await announcementRepository.GetAnnouncements();
            ViewData["role"] = credential.id_role;
            ViewData["picture"] = credential.path_image;
            ViewData["role_name"] = credential.role_name;
            return View(model);
        }
        public async Task<IActionResult> AnnouncementDetail(int id)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            var announcement = await  announcementRepository.ExistsAnnouncementById(id);

            if (announcement)
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = await announcementRepository.GetAnnouncementById(id);
                return View(model);
            }
            else
            {
                return RedirectToAction("e404", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> CreateAnnouncement()
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var modelo = new AnnouncementCreationViewModel();
                return View(modelo);
            }

        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(AnnouncementCreationViewModel announcement)
        {
            if (!ModelState.IsValid)
            {
                return View(announcement);
            }
            var path = "wwwroot/SystemPictures/AnnouncementPictures";
            var file_location = "SystemPictures/AnnouncementPictures";
            Random rand = new Random();
            int number = rand.Next(0, 10000);
            announcement.announcement_picture = await UploadFile(path, announcement.announcement_picture_file, announcement.announcement_name + number, file_location);
            await announcementRepository.CreateAnnouncement(announcement);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditAnnouncement(int id)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                AnnouncementCreationViewModel announcement = await announcementRepository.GetAnnouncementById(id);
                if (announcement is null) { return RedirectToAction("WhereDoYouGo", "Home"); }
                return View(announcement);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditAnnouncement(AnnouncementCreationViewModel announcement)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (!ModelState.IsValid) { return View(); }
            var exists = await announcementRepository.ExistsAnnouncement(announcement);
            if (exists)
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var model = announcement;
                ModelState.AddModelError(nameof(announcement.announcement_name),
                    "Ya hay una convocatoria con los mismos datos.");
                return View(model);
            }
            await announcementRepository.UpdateAnnouncement(announcement);
            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> DeleteAnnouncementConfirmation(int id)
        {
            var student_id = userService.GetUserId();
            var credential = new Credential();
            credential = await credentials.GetCredentials(student_id);
            if (credential.id_role != 1 && credential.id_role != 2)
            {
                return RedirectToAction("e404", "Home");
            }
            else
            {
                ViewData["role"] = credential.id_role;
                ViewData["picture"] = credential.path_image;
                ViewData["role_name"] = credential.role_name;
                var announcement = await announcementRepository.GetAnnouncementById(id);
                if (announcement is null) { return RedirectToAction("WhereDoYouGo", "Home"); }
                return View(announcement);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAnnouncement(int id_announcement)
        {
            var announcement = await announcementRepository.GetAnnouncementById(id_announcement);
            if (announcement is null) { return RedirectToAction("e404", "Home"); }
            DeleteExistingFile(announcement.announcement_picture);
            await announcementRepository.DeleteAnnouncementById(id_announcement);
            return RedirectToAction("Index");
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
        public void DeleteExistingFile(string db_path)
        {
            string path = System.IO.Path.Combine(webHostEnvironment.ContentRootPath,
                "wwwroot/" + db_path);
            System.IO.File.Delete(path);
        }
    }
}
