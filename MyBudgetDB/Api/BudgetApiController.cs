using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;

namespace MyBudgetDB.Api
{
    [Produces("application/json")]
    [Route("api/BudgetApi")]
    //[ValidateModel, HandleException, FeatureEnabled(IsEnabled = true)]
    public class BudgetApiController : Controller
    {
        public BudgetService _budgetService;
        private readonly ILogger _log;

        public BudgetApiController(BudgetService service, ILogger<BudgetApiController> log)
        {
            _budgetService = service;
            _log = log;
        }

        // POST: api/BudgetApi
        //[HttpPost]
        //public IActionResult Post([FromBody]CreateBudgetCommand value)
        //{
        //    var appUser = await _userService.GetUserAsync(User);
        //    var id = _service.CreateBudget(value, appUser);

        //    return Ok(new { id = id });
        //}
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var detail = _budgetService.GetBudgetDetail(id);
            return Ok(detail);

        }

        [HttpPost("{id}")]// EnsureRecipeExists, RequireIpAddress
        public IActionResult Edit(int id, [FromBody] UpdateBudgetCommand command)
        {
            _budgetService.UpdateBudget(command);
            return Ok();
        }
    }
}
