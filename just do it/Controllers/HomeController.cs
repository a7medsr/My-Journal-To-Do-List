using just_do_it.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace just_do_it.Controllers
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
            // Redirect to login if not authenticated
            return RedirectToAction("login", "Userlgsin");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
