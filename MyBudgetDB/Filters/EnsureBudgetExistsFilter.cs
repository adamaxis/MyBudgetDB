using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Models.BudgetCommands;
using MyBudgetDB.Services;

namespace MyBudgetDB.Filters
{
    public class EnsureBudgetExistsAttribute : TypeFilterAttribute
    {
        public EnsureBudgetExistsAttribute() : base(typeof(EnsureBudgetExistsFilter)) { }

        public class EnsureBudgetExistsFilter : IActionFilter
        {
            private readonly BudgetService _service;
            private readonly ILogger _log;

            public EnsureBudgetExistsFilter(BudgetService service, ILogger log)
            {
                _service = service;
                _log = log;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                var budgetId = (int)context.ActionArguments["id"];
                if (!_service.DoesBudgetExist(budgetId))
                {
                    context.Result = new NotFoundResult();
                }
            }

            public void OnActionExecuted(ActionExecutedContext context) { }
        }
    }
}
