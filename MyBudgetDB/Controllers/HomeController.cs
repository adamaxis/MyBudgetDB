using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBudgetDB.Data;
using MyBudgetDB.Models;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;

namespace MyBudgetDB.Controllers
{
    public class HomeController : Controller
    {
        public BudgetService _service;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly IAuthorizationService _authService;

        public HomeController(
            BudgetService service,
            UserManager<ApplicationUser> userService,
            IAuthorizationService authService)
        {
            _service = service;
            _userService = userService;
            _authService = authService;
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

        [Authorize]
        public IActionResult CreateBudget()
        {
            return View(new CreateBudgetCommand());
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> CreateBudget(CreateBudgetCommand command)
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var id = _service.CreateBudget(command, user);
                    return RedirectToAction(nameof(ViewBudgets));
                }//, new { id = id }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occured while trying to connect to the database");
            }

            return View(command);
        }

        [Authorize]
        public async Task<IActionResult> ViewBudgets()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            var budgets = _service.GetBudgetsBrief(user.Id);
            return View(budgets);
        }

        [Authorize]
        public async Task<IActionResult> ViewBudget(int id)
        {
            var model = _service.GetBudgetDetail(id);

            if (model == null)
            {
                return NotFound();
            }

            var budget = _service.GetBudget(id);

            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> EditBudget(int id)
        {
            var model = _service.GetBudgetDetail(id);

            if (model == null)
            {
                return NotFound();
            }

            var budget = _service.GetBudget(id);

            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
