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
    public class BudgetApiController : Controller
    {
        public BudgetService _budgetService;
        private readonly ILogger _log;

        public BudgetApiController(BudgetService service, ILogger<BudgetApiController> log)
        {
            _budgetService = service;
            _log = log;
        }

        // GET: api/BudgetApi
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/BudgetApi/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/BudgetApi
        [HttpPost]
        public IActionResult Post([FromBody]CreateBudgetCommand value)
        {
            //var id = 
                _budgetService.CreateBudget(value);

            return Ok();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
