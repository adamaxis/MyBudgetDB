using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Data;
using MyBudgetDB.Models;
using MyBudgetDB.Services;

namespace MyBudgetDB.Controllers
{
    public class HomeController : Controller
    {
        public BudgetService _service;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly IAuthorizationService _authService;
        private readonly ILogger _log;

        public HomeController(
            BudgetService service,
            UserManager<ApplicationUser> userService,
            IAuthorizationService authService,
            ILogger<HomeController> log)
        {
            _service = service;
            _userService = userService;
            _authService = authService;
            _log = log;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
