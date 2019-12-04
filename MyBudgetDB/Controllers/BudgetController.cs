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
    [Authorize]
    public class BudgetController : Controller
    {
        public BudgetService _service;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly IAuthorizationService _authService;

        public BudgetController(
            BudgetService service,
            UserManager<ApplicationUser> userService,
            IAuthorizationService authService)
        {
            _service = service;
            _userService = userService;
            _authService = authService;
        }

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

        [HttpPost]
        public async Task<IActionResult> EditBudget(UpdateBudgetCommand command)
        {
            try
            {
                var budget = _service.GetBudget(command.BudgetId);
                var authResult = await _authService.AuthorizeAsync(User, budget, "CanManageBudget");
                if (!authResult.Succeeded)
                {
                    return new ForbidResult();
                }

                if (ModelState.IsValid)
                {
                    _service.UpdateBudget(command);
                    return RedirectToAction(nameof(ViewBudget), new { id = command.BudgetId });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occured while trying to connect to the database.");
            }

            return View(command);
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}