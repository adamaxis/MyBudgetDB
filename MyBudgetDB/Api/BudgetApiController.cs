using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Attributes;
using MyBudgetDB.Data;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;

namespace MyBudgetDB.Api
{
    [RequireHttps]
    [Route("api/BudgetApi"), FormatFilter]
    [FeatureEnabled(IsEnabled = true), 
     HandleException]
    [Authorize]
    public class BudgetApiController : Controller
    {
        private readonly BudgetService _budgetService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authService;
        private readonly ILogger _log;
        
        public BudgetApiController(
            BudgetService service, 
            ILogger<BudgetApiController> log,
            IAuthorizationService authService,
            UserManager<ApplicationUser> userManager)
        {
            _budgetService = service;
            _log = log;
            _userManager = userManager;
        }

        [ResponseCache(NoStore = true)]
        [HttpGet("GetBudgets/{format}")]
        public async Task<IActionResult> GetBudgets()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var budgets = _budgetService.GetBudgetsBrief(user.Id);
            return Ok(budgets);
        }

        [HttpGet("GetBy/{id}/{format}"), EnsureBudgetExist]
        public IActionResult GetById(int id)
        {
            var budget = _budgetService.GetBudget(id);
            return Ok(budget);
        }

        [HttpGet("GetUser/{format}")]
        public async Task<IActionResult> GetUserDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(user);
        }

        [ValidateModel, AddLastModifiedHeader]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateBudgetCommand cmd)
        {
            var user = await _userManager.GetUserAsync(User);
            var id = _budgetService.CreateBudget(cmd, user);

            return Ok(new { message = "Your budget id: " + id });
        }

        [EnsureBudgetExist, AddLastModifiedHeader, ValidateModel]
        [HttpPost("edit/{id}")] //need the id parameter to check EnsureBudgetExistAttribute
        public IActionResult Edit(int id, [FromBody] UpdateBudgetCommand cmd)
        {
            // this fires an error onExecuted in EnsureBudget Exist Object reference not set to an instance of an object."
            //var budget = _budgetService.GetBudget(id);
            //var authResult = await _authService.AuthorizeAsync(User, budget, "CanEditPerson");

            //if (!authResult.Succeeded)
            //{
            //    return new ForbidResult();
            //}

            _budgetService.UpdateBudget(cmd);
            var newBudget = _budgetService.GetBudget(cmd.BudgetId);
            return Ok(newBudget);
        }

        [HttpDelete("delete /{id}"), EnsureBudgetExist, AddLastModifiedHeader]
        public IActionResult Delete(int id)
        {
            _budgetService.DeleteBudget(id);

            return Ok();
        }
    }
}
