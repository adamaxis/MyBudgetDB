using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Attributes;
using MyBudgetDB.Data;
using MyBudgetDB.Models;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;

namespace MyBudgetDB.Api
{
    [Produces("application/json")]
    [Route("api/BudgetApi")]
    [ValidateModel, HandleException, FeatureEnabled(IsEnabled = true)]
    [ServiceFilter(typeof(HandleExceptionAttribute))]
    [Authorize]
    public class BudgetApiController : Controller
    {
        public BudgetService _budgetService;
        public UserService _userService;
        private readonly ILogger _log;

        public BudgetApiController(BudgetService service, ILogger<BudgetApiController> log)
        {
            _budgetService = service;
            _log = log;
        }

        [HttpGet("{id}"), EnsureBudgetExist, AddLastModifiedHeader]
        public IActionResult Get(int id)
        {
            var detail = _budgetService.GetBudgetDetail(id);
            return Ok(detail);

        }

        [HttpPost("{id}/{userId}/{userName}"),ValidateModel, EnsureBudgetExist, ValidateUser]
        public IActionResult Edit(int id, [FromBody] UpdateBudgetCommand command)
        {
            _budgetService.UpdateBudget(command);
            return Ok();
        }

        // POST: api/BudgetApi
        [HttpPost("create"), ValidateModel]
        public IActionResult Create([FromBody]CreateBudgetCommand value, ApplicationUser createdBy)
        {
            var id = _budgetService.CreateBudget(value, createdBy);

            return Ok(new { id = id });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]CreateUser user)
        {
            var id = _userService.CreateUser(user);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete /{id}"), ValidateModel, EnsureBudgetExist, AddLastModifiedHeader]
        public IActionResult Delete(int id)
        {
            _budgetService.DeleteBudget(id);

            return Ok();
        }
    }
}
