using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Attributes;
using MyBudgetDB.Authorization;
using MyBudgetDB.Data;
using MyBudgetDB.Filters;
using MyBudgetDB.Models;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;

namespace MyBudgetDB.Controllers
{
    [Authorize, LogRequestAsync(IsEnabled = true)]
    [RequireHttps, HandleException, ValidateUser]
    public class BudgetController : Controller
    {
        public BudgetService _service;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly IAuthorizationService _authService;
        private readonly ILogger _log;

        public BudgetController(
            BudgetService service,
            UserManager<ApplicationUser> userService,
            IAuthorizationService authService,
            ILogger<BudgetController> log)
        {
            _service = service;
            _userService = userService;
            _authService = authService;
            _log = log;
        }


        public IActionResult CreateBudget()
        {
            return View(new CreateBudgetCommand());
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateBudget(CreateBudgetCommand command)
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                _log.LogWarning($"Unable to load user with ID '{_userService.GetUserId(User)}'. Info:{command}");
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                var id = _service.CreateBudget(command, user);
                return RedirectToAction(nameof(ViewBudget), new { id = id });
            }
            return View(command);
        }
        
        public async Task<IActionResult> ViewBudgets()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                _log.LogWarning($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
                return Forbid();
            }

            // show budgets(user-specific for user; all for admin)
            var budgets = _service.GetBudgetsBrief(user.Id, User.HasClaim(c => c.Type == Claims.IsAdmin));

            return View(budgets);
        }

        public async Task<IActionResult> ViewBudget(int id)
        {
            var model = _service.GetBudgetDetail(id);
            if (model == null)
            {
                _log.LogWarning($"{_userService.GetUserName(User)} tried to view budget#{id}, but it wasn't found.");
                return NotFound();
            }

            // Authorization
            var budget = _service.GetBudget(id);
            var authResult = await _authService.AuthorizeAsync(User, budget, "CanViewBudget");
            if(!authResult.Succeeded)
            {
                return Forbid();
            }
            model.CanEditBudget = true;
            return View(model);
        }


        public async Task<IActionResult> EditBudget(int id)
        {

            // Add this for authorization
            var budget = _service.GetBudget(id);
            var authResult = await _authService.AuthorizeAsync(User, budget, "CanViewBudget");
            
            if (!authResult.Succeeded)
            {
                _log.LogWarning($"{_userService.GetUserName(User)} went to edit budget#{id}, but wasn't authorized.");
                return new ForbidResult();
            }

            var model = _service.GetBudgetForUpdate(id);
            if (model == null)
            {
                _log.LogWarning($"User with ID '{_userService.GetUserName(User)}' went to edit Budget #{id}, but it wasn't found.");
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditBudget(UpdateBudgetCommand command)
        {
            var budget = _service.GetBudget(command.BudgetId);
            var authResult = await _authService.AuthorizeAsync(User, budget, "CanViewBudget");
            if (!authResult.Succeeded)
            {
                _log.LogWarning($"{_userService.GetUserName(User)} attempted to change budget#{command.BudgetId}, but wasn't authorized.");
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(command);
            }

            _service.UpdateBudget(command);
            return RedirectToAction(nameof(ViewBudget), new { id = command.BudgetId });
        }

        public async Task<IActionResult> DeleteBudget(int id)
        {
            // Authorization
            var budget = _service.GetBudget(id);
            var authResult = await _authService.AuthorizeAsync(User, budget, "CanViewBudget");
            if (!authResult.Succeeded)
            {
                _log.LogWarning($"{_userService.GetUserName(User)} tried to delete budget #{id}, but wasn't authorized.");
                return Forbid();
            }

            var model = _service.GetBudgetDetail(id);
            if (!_service.DoesBudgetExist(id))
            {
                _log.LogWarning($"User with ID '{_userService.GetUserName(User)}' attempted to delete Budget #{id}, but it wasn't found.");
                return NotFound();
            }

            _service.DeleteBudget(id);
            return RedirectToAction(nameof(ViewBudgets));
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}