using Microsoft.AspNetCore.Mvc;
using SIEL_1836109025062022.Models;
using System.Diagnostics;

namespace SIEL_1836109025062022.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "User");
        }

        public IActionResult Errore()
        {
            return View();
        }
        public IActionResult e404()
        {
            return View();
        }

        public IActionResult WhereDoYouGo()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}