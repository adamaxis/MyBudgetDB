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
using Newtonsoft.Json;

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
            var budget = _service.GetBudget(id);
            var authResult = await _authService.AuthorizeAsync(User, budget, "CanManageBudget");

            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }
            
            var model = _service.GetBudgetDetail(id);

            if (model == null)
            {
                return NotFound();
            }

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
            // Add this for authorization
            var budget = _service.GetBudget(id);
            var authResult = await _authService.AuthorizeAsync(User, budget, "CanManageBudget");

            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            var model = _service.GetBudgetForUpdate(id);
            if (model == null)
            {
                return NotFound();
            }
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }
            return View(model);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> EditBudget(UpdateBudgetCommand command)
        {
            try
            {
                var person = _service.GetBudget(command.BudgetId);
                /*var authResult = await _authService.AuthorizeAsync(User, person, "CanEditPerson");
                if (!authResult.Succeeded)
                {
                    return new ForbidResult();
                }*/

                if (ModelState.IsValid)
                {
                    _service.UpdateBudget(command);
                    return RedirectToAction(nameof(ViewBudget), new { id = command.BudgetId });
                }
            }
            catch (Exception)
            {
                //            throw new Exception($"{JsonConvert.SerializeObject(budget)}");
                ModelState.AddModelError(string.Empty, "An error occured while trying to connect to the database.");
            }

            return View(command);
        }

        [Authorize]
        public IActionResult DeleteBudget(int id)
        {
            try
            {
                _service.DeleteBudget(id);
                return RedirectToAction(nameof(ViewBudgets));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
