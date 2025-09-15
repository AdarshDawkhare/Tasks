using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tasks.Models;

namespace Tasks.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult ReturnRegisterView()
        {
            return View("Register");
        }

        [HttpGet]
        public IActionResult ReturnLoginView()
        {
            return View("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ReturnLandingPage()
        {
            return View("LandingPage");
        }

    }
}
